using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace EasySave
{
    internal class Config
    {
        public Config()
        {
            // Test settings is empty
            if (ConfigurationManager.AppSettings.Count == 0)
            {
                AddUpdateAppSettings("Langue", "FR");
            }
        }

        public static void ReadAllSettings()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    Console.WriteLine("AppSettings is empty.");
                }
                else
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        Console.WriteLine("Key: {0}   |   Value: {1}", key, appSettings[key]);
                    }
                }
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
        }

        public static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                Console.WriteLine(result); //TODO: Remove
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
            return string.Empty;
        }

        public static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        public static void AddJobIntoConfig(SavingJob savingJob)
        {
            String JobListStr = ReadSetting("JobList");

            if(JobListStr.Length == 0)
            {
                AddUpdateAppSettings("JobList", savingJob.ToString());
            }
            else
            {
                AddUpdateAppSettings("JobList", JobListStr + ">" + savingJob.ToString());
            }
        }

        public static List<SavingJob> GetSavingJobs()
        {
            List<SavingJob> savingJobs = new List<SavingJob>();
            String JobListStr = ReadSetting("JobList");

            if (JobListStr.Length == 0)
            {
                return savingJobs;
            }
            else
            {
                String[] JobList = JobListStr.Split('>');

                foreach (String job in JobList)
                {
                    SavingJob savingJob = new SavingJob(job);
                    savingJobs.Add(savingJob);
                }
            }

            return savingJobs;
        }
    }
}
