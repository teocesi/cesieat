using EasySave.utils;
using Newtonsoft.Json;
using System.Windows;

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
        private static List<Job> LoadJobList(string jobListStr)
        {
            try
            {
                return Config.ReadSetting("JobList") == "" ? new List<Job>() :
                    JsonConvert.DeserializeObject<List<Job>>(jobListStr);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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
        public static void RemoveJob(string jobName)
        {
            RemoveJob(getJobByName(jobName));
        }
        public static IEnumerable<string> GetJobNames()
        {
            return JobList.jobList.Select(job => job.Name);
        }
        public static Job getJobByName(string jobName)
        {
            return jobList.Find(job => job.Name == jobName);
        }
        public static void UpdateJobState(string jobName, byte state)
        {
            Job job = getJobByName(jobName);
            job.State = state;
            UpdateJobListConfig();
        }
    }
}