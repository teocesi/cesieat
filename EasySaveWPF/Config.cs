using System.Configuration;
using System.Windows;
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
                AddUpdateAppSettings("businessSoft", "calc.exe");
                AddUpdateAppSettings("cryptExt", ".txt");
                AddUpdateAppSettings("priority", "");
                AddUpdateAppSettings("cryptExePath", @"C:\Users\selya\Desktop\CryptoSoft\CryptoSoft.exe");
                AddUpdateAppSettings("JobList", "");
                AddUpdateAppSettings("LogType", "json");
                AddUpdateAppSettings("LogPath", "");
            }
            Language.SetLangue(ReadSetting("Language"));
        }

        // Allow to get the instance of the class (Singleton)
        public static Config GetInstance()
        {
            if (config == null)
            {
                config = new Config();
            }
            return config;
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
                MessageBox.Show("Error reading app settings");
            }
            return string.Empty;
        }

        // Allow to add or update a setting by key
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
                MessageBox.Show("Error writing app settings");
            }
        }
    }
}
