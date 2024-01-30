using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave
{
    internal class FileExplorer
    {
        private IEnumerable<String> files;

        public FileExplorer(string sourcePath)
        {
            GetFiles(sourcePath);
        }

        public string[] GetFiles(string sourcePath)
        {
            files = Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories);

            if (files.Count() == 0)
            {
                Console.WriteLine("No files found.");
                return null;
            }

            Console.WriteLine(String.Join(Environment.NewLine, files));
            //foreach (string file in files)
            //{
            //    Console.WriteLine(file);
            //}

            return null;
        }
    }
}
