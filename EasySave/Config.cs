using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                AddUpdateAppSettings("JobList", "");
                Console.WriteLine("Choose a log path:");
                AddUpdateAppSettings("LogPath", Console.ReadLine());
            }
            Language.SetLangue(ReadSetting("Language"));

            // Create log file
            try
            {
                if (!Directory.Exists(ReadSetting("LogPath")))
                {
                    Directory.CreateDirectory(ReadSetting("LogPath"));
                }
                if (!File.Exists(ReadSetting("LogPath") + "/status.log"))
                {
                    File.Create(ReadSetting("LogPath") + "/status.log");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
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
    }
}
