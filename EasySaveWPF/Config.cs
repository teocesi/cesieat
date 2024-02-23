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
                AddUpdateAppSettings("koLimit", "2500000");
                AddUpdateAppSettings("priority", "");
                AddUpdateAppSettings("cryptExt", ".txt");
                AddUpdateAppSettings("cryptKey", "123456789");
                AddUpdateAppSettings("cryptExePath", "");
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
        public static string ReadAllSettings()
        {
            string result = "";
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                foreach (var key in appSettings.AllKeys)
                {
                    string tab = key.Length <= 9 ? "\t\t" : "\t";
                    result += string.Format("Key: {0}{1}|\tValue: {2}\n", key, tab, appSettings[key]);
                }
            }
            catch (ConfigurationErrorsException)
            {
                result = "Error reading app settings";
            }

            return result;
        }
        public static IEnumerable<string> GetOptionNames()
        {
            var appSettings = ConfigurationManager.AppSettings;
            return appSettings.AllKeys;
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
