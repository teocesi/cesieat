using System;
using System.Collections.Generic;
using System.Linq;
using EasySave.utils;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.AccessControl;
using System.IO;
using System.Windows;

namespace EasySave.model
{
    internal class Job // Model to store the job, run it or convert it to JSON
    {
        public string Name;
        public List<string> SourcePaths;
        public string DestinationPath;
        public bool IsDifferential;
        public byte Priority;
        public byte State; // 0 = stopped, 1 = running, 2 = paused
        Job() { }
        public Job(String name, List<String> sourcePaths, String destinationPath, bool IsDifferential, byte priority, byte state)
        {
            this.Name = name;
            this.SourcePaths = sourcePaths;
            this.DestinationPath = destinationPath;
            this.IsDifferential = IsDifferential;
            this.Priority = priority;
            this.State = state;
        }

        // Running
        public void Run()
        {
            foreach (String sourcePath in this.SourcePaths)
            {
                FileExplorer fileExplorer = new FileExplorer(sourcePath, this);
                if (this.IsDifferential)
                {
                    fileExplorer.GetDiffFilesPath();
                }
                else
                {
                    FileExplorer.EmptyDirectory(this.DestinationPath + "\\" + new DirectoryInfo(sourcePath).Name);
                    fileExplorer.GetAllFilesPath();
                }
                fileExplorer.CopyAllFiles();
            }

            // Job finished
            LogBuilder.UpdateStatusLog(this, 0, "null", "null");
            JobList.UpdateJobState(this.Name, 0);
        }
    }
}
