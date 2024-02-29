using EasySave.model;
using EasySave.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EasySave.utils
{
    internal class FileExplorer
    {
        public static readonly object PriorityStick = new object();
        private IEnumerable<String> files;
        private string sourcePath;
        private Job job;

        public FileExplorer(string sourcePath, Job job)
        {
            this.sourcePath = sourcePath;
            this.job = job;
        }

        // Check if the file exists in the target directory and if it is up to date
        static bool FileEquals(string SourcePath, string TargetPath)
        {
            if (!File.Exists(TargetPath)) { return false; }

            if (File.GetLastWriteTime(SourcePath) > File.GetLastWriteTime(TargetPath)) { return false; }

            return true;
        }

        // Return all the files path (string) in the source directory
        public IEnumerable<String> GetAllFilesPath()
        {
            try
            {
                files = Directory.EnumerateFiles(this.sourcePath, "*", SearchOption.AllDirectories);
                files = SortFiles(files);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return Enumerable.Empty<String>();
            }

            return files;
        }
        private IEnumerable<String> SortFiles(IEnumerable<String> files)
        {
            IEnumerable<string> priorityExtension = Config.ReadSetting("priority").Split("\r\n");
            IEnumerable<string> priorityFiles = files.Where(f => priorityExtension.Contains(Path.GetExtension(f)));
            files = files.Except(priorityFiles);
            files = priorityFiles.Concat(files);

            //MessageBox.Show(string.Join("\n", files)); // Afficher liste des fichers en un string

            return files;
        }

        // Return the files path (string) that are different in the target directory
        public IEnumerable<String> GetDiffFilesPath()
        {
            IEnumerable<String> targetFiles = GetAllFilesPath();
            foreach (string file in targetFiles)
            {
                if (FileEquals(file, job.DestinationPath + file.Substring(Directory.GetParent(sourcePath).FullName.Length)))
                {
                    files = files.Where(f => f != file);
                }
            }

            return files;
        }

        // Copy all the files in the source directory to the target directory
        public void CopyAllFiles()
        {
            foreach (string file in files ?? Enumerable.Empty<String>())
            {
                if (job.State == 0) { Thread.CurrentThread.Interrupt(); return; }
                string desFile = job.DestinationPath + file.Substring(Directory.GetParent(sourcePath).FullName.Length);
                CopyFile(file, desFile);
            }
        }

        // Copy a file from the source path to the target path
        private void CopyFile(string srcPath, string desPath)
        {
            // Stop the copy if the user wants to stop the job
            do
            {
                Thread.Sleep(10);
                LogBuilder.UpdateStatusLog(job, srcPath, desPath);
            }
            while (job.State == 2 || StopCopySystem.BusinessSoftLunched());

            // Priority system for file > N Ko
            bool _lockTaken = false;
            if (new FileInfo(srcPath).Length > (long.Parse(Config.ReadSetting("koLimit")) * 1000))
            {
                Monitor.Enter(PriorityStick);
                _lockTaken = true;
            }

            try
            {
                TimeWatcher timeWatcher = new TimeWatcher();
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(desPath));
                }
                catch (Exception e)
                {
                    MessageBox.Show("Unreachable directory path: " + e.Message);
                    return;
                }

                // Set priority of the process and thread
                string ext = Path.GetExtension(srcPath);
                if (Config.ReadSetting("priority").Split("\r\n").Contains(ext) || job.Priority)
                {
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
                    Thread.CurrentThread.Priority = ThreadPriority.Highest;
                }
                else
                {
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.BelowNormal;
                    Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
                }

                // Check if the file needs to be encrypted
                string crypTime = "0";
                if (Config.ReadSetting("cryptExt").Split("\r\n").Contains(ext))
                {
                    crypTime = EncryptionXORCopy(srcPath, desPath);
                }
                else
                {
                    NoEncryptionCopy(srcPath, desPath);
                }

                // Get size of file
                string size = "NaN";
                try
                {
                    FileInfo fileInfo = new FileInfo(desPath);
                    size = fileInfo.Length.ToString() + " bytes";
                }
                catch (Exception e)
                {
                    MessageBox.Show("File not copied: " + e.Message);
                }

                LogBuilder.UpdateHistoryLog(this.job.Name, srcPath, desPath, size, timeWatcher.Stop(), crypTime);
            }
            finally
            {
                if (_lockTaken) { Monitor.Exit(PriorityStick); }
            }

        }

        // Clear target directory
        public static void EmptyDirectory(string directoryPath)
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(directoryPath);
                foreach (FileInfo file in directory.GetFiles()) file.Delete();
                foreach (DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
            }
            catch { }
        }

        // Copy type
        private string EncryptionXORCopy(string srcPath, string desPath)
        {
            try
            {
                TimeWatcher timeWatcherCryp = new TimeWatcher();
                string key = Config.ReadSetting("cryptKey");
                string exePath = Config.ReadSetting("cryptExePath");

                ProcessStartInfo procInfo = new ProcessStartInfo();
                procInfo.FileName = exePath;
                procInfo.CreateNoWindow = true; //Hides console
                procInfo.Arguments = $"\"{srcPath}\" \"{desPath}\" \"{key}\"";

                var proc = Process.Start(procInfo);

                proc.WaitForExit();
                proc.CloseMainWindow();
                proc.Close();

                return timeWatcherCryp.Stop();
            }
            catch
            {
                MessageBox.Show("Unreachable Cryptosoft path.");
                return "-1";
            }
        }
        private void NoEncryptionCopy(string srcPath, string desPath)
        {
            byte[] buffer = new byte[2048 * 2048];
            int bytesRead = 0;

            try
            {
                using (FileStream sr = new FileStream(srcPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (BufferedStream srb = new BufferedStream(sr))
                using (FileStream sw = new FileStream(desPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                using (BufferedStream swb = new BufferedStream(sw))
                {
                    while (true)
                    {
                        bytesRead = srb.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0) break;
                        swb.Write(buffer, 0, bytesRead);
                    }
                    swb.Flush();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Unreachable File path: " + e.Message);
            }
        }
    }
}
