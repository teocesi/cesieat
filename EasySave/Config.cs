using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Globalization;
using System.Resources;
using EasySave.utils;

namespace EasySave
{
    internal class Config // Singleton
    {
        private static Config config;

        private Config()
        {
            // Test settings is empty
            if (ConfigurationManager.AppSettings.Count == 0)
            {
                AddUpdateAppSettings("Language", "en");
                Console.WriteLine("Choose a log path:");
                AddUpdateAppSettings("LogPath", Console.ReadLine());
            }
            else
            {
                Language.SetLangue(ReadSetting("Language"));
            }
        }

        public static Config GetInstance()
        {
            if (config == null)
            {
                config = new Config();
            }
            return config;
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

        // Job config
        public static void AddJobIntoConfig(SavingJob savingJob)
        {
            String JobListStr = ReadSetting("JobList");

            if (JobListStr.Length == 0)
            {
                AddUpdateAppSettings("JobList", savingJob.ToString());
            }
            else
            {
                AddUpdateAppSettings("JobList", JobListStr + ">" + savingJob.ToString());
            }
        }
        public static void DeleteJobIntoConfig(SavingJob savingJob)
        {
            SavingJob[] savingJobs = GetSavingJobs();

            String JobListStr = "";

            for (int i = 0; i < savingJobs.Length; i++)
            {
                if (savingJobs[i].GetName() != savingJob.GetName())
                {
                    if (JobListStr.Length == 0)
                    {
                        JobListStr = savingJobs[i].ToString();
                    }
                    else
                    {
                        JobListStr += ">" + savingJobs[i].ToString();
                    }
                }
            }

            AddUpdateAppSettings("JobList", JobListStr);
        }
        public static SavingJob[] GetSavingJobs()
        {
            String JobListStr = ReadSetting("JobList");
            String[] JobListTab = JobListStr.Split('>');
            SavingJob[] savingJobs = new SavingJob[JobListTab.Length];

            if (JobListStr.Length == 0)
            {
                return new SavingJob[0];
            }
            else
            {
                for (int i = 0; i < JobListTab.Length; i++)
                {
                    savingJobs[i] = new SavingJob(JobListTab[i]);
                }
            }

            return savingJobs;
        }
    }
}
