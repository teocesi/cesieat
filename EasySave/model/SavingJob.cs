using System;
using System.Collections.Generic;
using System.Linq;
using EasySave.utils;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    internal class SavingJob
    {
        private string Name;
        private List<String> SourcePaths;
        private string DestinationPath;
        private bool IsDifferential;
        private int Priority;
        private int State;

        public SavingJob(String name, List<String> sourcePaths, String destinationPath, bool IsDifferential, int priority, int state)
        {
            this.Name = name;
            this.SourcePaths = sourcePaths;
            this.DestinationPath = destinationPath;
            this.IsDifferential = IsDifferential;
            this.Priority = priority;
            this.State = state;
        }
        public SavingJob(String SavingJobStr)
        {
            string[] jobConfigTab = SavingJobStr.Split('|');

            try
            {
                this.Name = jobConfigTab[0];
                this.SourcePaths = jobConfigTab[1].Split(',').ToList();
                this.DestinationPath = jobConfigTab[2];
                this.IsDifferential = Boolean.Parse(jobConfigTab[3]);
                this.Priority = Int32.Parse(jobConfigTab[4]);
                this.State = Int32.Parse(jobConfigTab[5]);
            }
            catch (Exception)
            {
                throw new Exception("Error while parsing SavingJob string");
            }
        }

        public void AddSourcePath(String sourcePath)
        {
            this.SourcePaths.Add(sourcePath);
        }

        public void RemoveSourcePath(String sourcePath)
        {
            this.SourcePaths.Remove(sourcePath);
        }

        public override String ToString()
        {
            return $"{this.Name}|{string.Join(",", this.SourcePaths)}|{this.DestinationPath}|{this.IsDifferential}|{this.Priority}|{this.State}";
        }

        public static void ShowJobs(SavingJob[] jobs)
        {
            Console.WriteLine(Language.GetText("job_list"));
            if (jobs.Length == 0)
            {
                Console.WriteLine(Language.GetText("no_job"));
            }
            for (int i = 0; i < jobs.Length; i++)
            {
                Console.WriteLine(i + ") " + jobs[i].Name);
            }
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

                FileExplorer fileExplorer = new FileExplorer(sourcePath);
                if (this.IsDifferential)
                {
                    fileExplorer.GetDiffFilesPath(this.DestinationPath);
                }
                else
                {
                    fileExplorer.GetAllFilesPath();
                }
                fileExplorer.CopyAllFiles(this.DestinationPath);

                Console.WriteLine($"> {Language.GetText("copying_finished")}");
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
