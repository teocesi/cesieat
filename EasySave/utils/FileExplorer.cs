using EasySave.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        static bool FileEquals(string SourcePath, string TargetPath)
        {
            if (!File.Exists(TargetPath)) { return false; }

            if (File.GetLastWriteTime(SourcePath) > File.GetLastWriteTime(TargetPath)) { return false; }

            return true;
        }

        public IEnumerable<String> GetAllFilesPath()
        {
            try
            {
                files = Directory.EnumerateFiles(this.sourcePath, "*", SearchOption.AllDirectories);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Enumerable.Empty<String>();
            }

            if (files.Count() == 0)
            {
                Console.WriteLine(Language.GetText("no_file"));
                return Enumerable.Empty<String>();
            }

            return files;
        }

        public IEnumerable<String> GetDiffFilesPath(string targetPath)
        {
            IEnumerable<String> targetFiles = GetAllFilesPath();
            foreach (string file in targetFiles)
            {
                if (FileEquals(file, targetPath + file.Substring(Directory.GetParent(sourcePath).FullName.Length)))
                {
                    files = files.Where(f => f != file);
                }
            }

            //Console.WriteLine(String.Join(Environment.NewLine, files));
            return files;
        }

        public void CopyAllFiles(string destinationPath)
        {
            foreach (string file in files ?? Enumerable.Empty<String>())
            {
                string desFile = destinationPath + file.Substring(Directory.GetParent(sourcePath).FullName.Length);
                Console.WriteLine("Copying " + file);
                CopyFile(file, desFile);
            }
        }

        private void CopyFile(string srcPath, string desPath)
        {
            TimeWatcher timeWatcher = new TimeWatcher();
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(desPath));
            }
            catch (Exception e)
            {
                Console.WriteLine("Unreachable path: " + e.Message);
            }

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
                Console.WriteLine("Unreachable File path: " + e.Message);
            }

            // Get size of file
            FileInfo fileInfo = new FileInfo(desPath);
            string size = fileInfo.Length.ToString() + " bytes";

            LogBuilder.UpdateHistoryLog(this.job.Name, srcPath, desPath, size, timeWatcher.Stop());
        }

        public static void EmptyDirectory(string directoryPath)
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(directoryPath);
                foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
                foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
            }
            catch { }
        }
    }
}
