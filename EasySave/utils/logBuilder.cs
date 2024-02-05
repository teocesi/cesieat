using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasySave.utils
{
    internal class LogBuilder
    {
        private static string BuildPathLog()
        {
            return Config.ReadSetting("LogPath") + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".json";
        }
        private static void WriteLog(string log)
        {
            try
            {
                System.IO.File.AppendAllText(BuildPathLog(), log);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static bool LogExists()
        {
            return System.IO.File.Exists(BuildPathLog());
        }

        public static void DailyLog(string jobName, string sourcePath, string targetPath, string size, string time)
        {
            string Horodatage = DateTime.Now.ToString("HH:mm:ss");
            string log = LogExists() ? ",\n" : "";

            log += "{\n" +
                "   \"Horodatage\": \"" + Horodatage + "\",\n" +
                "   \"JobName\": \"" + jobName + "\",\n" +
                "   \"SourcePath\": \"" + Regex.Replace(sourcePath, @"\\", "\\\\") + "\",\n" +
                "   \"TargetPath\": \"" + Regex.Replace(targetPath, @"\\", "\\\\") + "\",\n" +
                "   \"Size\": \"" + size + "\",\n" +
                "   \"Time\": \"" + time + "\"\n" +
                "}";

            WriteLog(log);
        }
    }
}
