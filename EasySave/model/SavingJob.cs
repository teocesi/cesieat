using System;
using System.Collections.Generic;
using System.Linq;
using EasySave.service.utils;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Model
{
    internal class SavingJob
    {
        public string Name { get; internal set; }
        public List<String> SourcePaths { get; internal set; }
        public string DestinationPath { get; internal set; }
        public int Type { get; internal set; }
        public int Priority { get; internal set; }
        public int State { get; internal set; }

        public SavingJob(String name, List<String> sourcePaths, String destinationPath, int type, int priority, int state)
        {
            this.Name = name;
            this.SourcePaths = sourcePaths;
            this.DestinationPath = destinationPath;
            this.Type = type;
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
                this.Type = Int32.Parse(jobConfigTab[3]);
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
            return $"{this.Name}|{string.Join(",", this.SourcePaths)}|{this.DestinationPath}|{this.Type}|{this.Priority}|{this.State}";
        }

        public static void ShowJobs(SavingJob[] jobs)
        {
            Console.WriteLine("List of jobs:");
            if (jobs.Length == 0)
            {
                Console.WriteLine("No jobs...");
            }
            for (int i = 0; i < jobs.Length; i++)
            {
                Console.WriteLine(i + ") " + jobs[i].Name);
            }
        }

        // Running
        public void Run()
        {
            switch (this.Type)
            {
                case 0:
                    this.RunFull();
                    break;
                case 1:
                    //this.RunDifferential();
                    break;
                default:
                    throw new Exception("Invalid job type");
            }
        }

        public void RunFull()
        {
            Console.WriteLine($"----- {this.Name} is running in full mode -----");
            foreach (String sourcePath in this.SourcePaths)
            {
                Console.WriteLine("> Source path: " + sourcePath);
                Console.WriteLine("> Destination path: " + this.DestinationPath);
                Console.WriteLine("> Copying files...");

                FileExplorer fileExplorer = new FileExplorer(sourcePath);
                fileExplorer.CopyAllFiles(this.DestinationPath);

                Console.WriteLine("> Copying files finished");
            }
            Console.WriteLine("Full job finished");
        }
    }
}
