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
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return Enumerable.Empty<String>();
            }

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
                string desFile = job.DestinationPath + file.Substring(Directory.GetParent(sourcePath).FullName.Length);
                CopyFile(file, desFile);
            }
        }

        // Copy a file from the source path to the target path
        private void CopyFile(string srcPath, string desPath)
        {
            // Security check
            while (SecurityStopSoftware.BusinessSoftLunched())
            {
                JobList.UpdateJobState(job.Name, 2);
                Thread.Sleep(1000);
            }
            JobList.UpdateJobState(job.Name, 1);

            TimeWatcher timeWatcher = new TimeWatcher();
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(desPath));
            }
            catch (Exception e)
            {
                MessageBox.Show("Unreachable path: " + e.Message);
            }

            // Check if the file needs to be encrypted
            string ext = Path.GetExtension(srcPath);
            string crypTime = "0";
            if (Config.ReadSetting("cryptExt").Split(',').Contains(ext))
            {
                crypTime = EncryptionXORCopy(srcPath, desPath);
            }
            else
            {
                NoEncryptionCopy(srcPath, desPath);
            }

            // Get size of file
            FileInfo fileInfo = new FileInfo(desPath);
            string size = fileInfo.Length.ToString() + " bytes";

            LogBuilder.UpdateHistoryLog(this.job.Name, srcPath, desPath, size, timeWatcher.Stop(), crypTime);
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
                string key = "123456";
                string exePath = Config.ReadSetting("cryptExePath");
                var proc = Process.Start(exePath, $"\"{srcPath}\" \"{desPath}\" \"{key}\"");

                proc.WaitForExit();
                proc.CloseMainWindow();
                proc.Close();

                LogBuilder.UpdateStatusLog(job, 1, srcPath, desPath);

                return timeWatcherCryp.Stop();
            }
            catch
            {
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
                    LogBuilder.UpdateStatusLog(job, 1, srcPath, desPath);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Unreachable File path: " + e.Message);
            }
        }
    }
}
