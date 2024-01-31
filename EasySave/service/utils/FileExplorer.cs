using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EasySave.service.utils
{
    internal class FileExplorer
    {
        private IEnumerable<String> files;
        private string sourcePath;

        public FileExplorer(string sourcePath)
        {
            this.sourcePath = sourcePath;
            GetFiles();
        }

        public string[] GetFiles()
        {
            files = Directory.EnumerateFiles(this.sourcePath, "*", SearchOption.AllDirectories);

            if (files.Count() == 0)
            {
                Console.WriteLine("No files found.");
                return null;
            }

            //Console.WriteLine(String.Join(Environment.NewLine, files));

            return null;
        }

        public void CopyAllFiles(string destinationPath)
        {
            foreach (string file in files)
            {
                string desFile = destinationPath + file.Substring(Directory.GetParent(sourcePath).FullName.Length);
                CopyFile(file, desFile);
            }
        }

        private void CopyFile(string srcPath, string desPath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(desPath));

            var buffer = new byte[1024 * 1024];
            var bytesRead = 0;

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
    }
}
