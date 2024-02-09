using EasySave.model;
using EasySave.utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EasySave.model
{
    internal static class JobList // Static class to manage the job list
    {
        public static List<Job> jobList = LoadJobList(Config.ReadSetting("JobList"));

        // Update config
        public static void UpdateJobListConfig()
        {
            Config.AddUpdateAppSettings("JobList", JobList.ToJson());
        }

        // Serialization
        public static string ToJson()
        {
            return JsonConvert.SerializeObject(jobList).ToString();
        }
        public static List<Job> LoadJobList(string jobListStr)
        {
            try
            {
                return Config.ReadSetting("JobList") == "" ? new List<Job>() :
                    JsonConvert.DeserializeObject<List<Job>>(jobListStr);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Job>();
            }
        }

        // CRUD
        public static void AddJob(Job job)
        {
            jobList.Add(job);
            UpdateJobListConfig();
        }
        public static void RemoveJob(Job job)
        {
            jobList.Remove(job);
            UpdateJobListConfig();
        }
        public static List<Job> GetJobList()
        {
            return jobList;
        }
        public static void ShowJobList()
        {
            Console.WriteLine(Language.GetText("job_list"));
            if (jobList.Count == 0)
            {
                Console.WriteLine(Language.GetText("no_job"));
            }
            for (int i = 0; i < jobList.Count; i++)
            {
                Console.WriteLine(i + ") " + jobList[i].GetName());
            }
        }
    }
}