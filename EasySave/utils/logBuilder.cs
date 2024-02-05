using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using EasySave.model;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
                File.AppendAllText(BuildPathLog(), log);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static bool IsLogFileExiste()
        {
            return File.Exists(BuildPathLog());
        }
        public static void UpdateHistoryLog(string jobName, string sourcePath, string targetPath, string size, string time)
        {
            HistoryLog executeLog = new HistoryLog
            {
                JobName = jobName,
                SourcePath = sourcePath,
                TargetPath = targetPath,
                Size = size,
                Time = time,
                Horodatage = DateTime.Now.ToString("dd/mm/yyyy HH:mm:ss")
            };

            if (IsLogFileExiste())
            {
                // Remove the last character of the file if it is a ']'
                try
                {
                    using (var fileStream = new FileStream(BuildPathLog(), FileMode.Open, FileAccess.ReadWrite))
                    {
                        fileStream.Position = fileStream.Seek(-1, SeekOrigin.End);
                        if (fileStream.ReadByte() == ']') { fileStream.SetLength(fileStream.Length - 1); }
                    }
                    WriteLog(",\n" + JsonSerializer.Serialize(executeLog) + "]");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                WriteLog("[" + JsonSerializer.Serialize(executeLog) + "]");
            }
        }
        public static void UpdateStatusLog(string jobName, string state, int totalFilesToCopy, int totalFilesSize, int nbFilesLeftToDo, int sizeFileLeft, string currentSourcePath, string currentTargetPath)
        {
            string logPath = Config.ReadSetting("LogPath") + "/status.log";

            try
            {
                // Get string in file to convert it to a list
                string fileContent = File.ReadAllText(logPath);

                List<StatusLog> statutList = JsonConvert.DeserializeObject<List<StatusLog>>(fileContent)
                    ?? new List<StatusLog>();

                StatusLog statusLogModel = new StatusLog
                {
                    JobName = jobName,
                    Horodatage = DateTime.Now.ToString("dd/mm/yyyy HH:mm:ss"),
                    State = state,
                    TotalFilesToCopy = totalFilesToCopy,
                    TotalSizeToCopy = totalFilesSize,
                    Progression = new string[] { nbFilesLeftToDo.ToString(), sizeFileLeft.ToString(), currentSourcePath, currentTargetPath }
                };

                // Add or update the status of the job
                if (statutList.Exists(x => x.JobName == jobName))
                {
                    int index = statutList.FindIndex(x => x.JobName == jobName);
                    statutList[index] = statusLogModel;
                }
                else
                {
                    statutList.Add(statusLogModel);
                }

                // Write the new list in the file
                File.WriteAllText(logPath, JsonSerializer.Serialize(statutList));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
