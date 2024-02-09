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

namespace EasySave.model
{
    internal class Job // Model to store the job, run it or convert it to JSON
    {
        public string Name;
        public List<string> SourcePaths;
        public string DestinationPath;
        public bool IsDifferential;
        public byte Priority;
        public byte State;
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

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this).ToString();
        }

        // Running
        public void Run()
        {
            Console.WriteLine($"----- {this.Name} {Language.GetText("is_running")} -----");
            Console.WriteLine($"> {Language.GetText("differential_mode")} {this.IsDifferential}");
            foreach (String sourcePath in this.SourcePaths)
            {
                Console.WriteLine($"> {Language.GetText("source_path")} {sourcePath}");
                Console.WriteLine($"> {Language.GetText("target_path")} {this.DestinationPath}");
                Console.WriteLine($"> {Language.GetText("copying_files")}");

                FileExplorer fileExplorer = new FileExplorer(sourcePath, this);
                if (this.IsDifferential)
                {
                    fileExplorer.GetDiffFilesPath(this.DestinationPath);
                }
                else
                {
                    FileExplorer.EmptyDirectory(this.DestinationPath + "\\" + new DirectoryInfo(this.SourcePaths[0]).Name);
                    fileExplorer.GetAllFilesPath();
                }
                fileExplorer.CopyAllFiles(this.DestinationPath);

                Console.WriteLine($"> {Language.GetText("copying_finished")}");
                LogBuilder.UpdateStatusLog(this, 0, "null", "null");
            }
            Console.WriteLine(Language.GetText("full_job_finished"));
        }

        // Getters
        public string GetName()
        {
            return this.Name;
        }
        public List<String> GetSourcePaths()
        {
            return this.SourcePaths;
        }
        public string GetDestinationPath()
        {
            return this.DestinationPath;
        }
        public bool GetIsDifferential()
        {
            return this.IsDifferential;
        }
        public int GetPriority()
        {
            return this.Priority;
        }
        public int GetState()
        {
            return this.State;
        }
    }
}
