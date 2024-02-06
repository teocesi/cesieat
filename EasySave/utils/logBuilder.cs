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
using System.Collections;

namespace EasySave.utils
{
    internal class LogBuilder
    {
        private static string BuildPathLog()
        {
            return Config.ReadSetting("LogPath") + "/logs/" + DateTime.Now.ToString("yyyy-MM-dd") + ".json";
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
                TimeTransfer = time,
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
        public static void UpdateStatusLog(Job job, int state, string currentSourcePath, string currentTargetPath)
        {
            try
            {
                // Set log path file
                string logPath = Config.ReadSetting("LogPath") + "/logs/status.json";
                if (!File.Exists(logPath))
                {
                    Directory.CreateDirectory(Config.ReadSetting("LogPath") + "/logs");
                    File.Create(logPath);
                }


                // Get directories size
                IEnumerable<FileInfo> totalFileSource = new List<FileInfo>();
                IEnumerable<FileInfo> totalFileTarget = new List<FileInfo>();
                foreach (var sourcePathTemp in job.SourcePaths)
                {
                    DirectoryInfo sourceDirInfo = new DirectoryInfo(sourcePathTemp);
                    totalFileSource = totalFileSource.Concat(sourceDirInfo.EnumerateFiles("*", SearchOption.AllDirectories));
                    //totalFilesSize += sourceDirInfo.EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);

                    var targetPath = job.DestinationPath + "\\" + new DirectoryInfo(sourcePathTemp).Name;
                    DirectoryInfo targetDirInfo = new DirectoryInfo(targetPath);
                    totalFileTarget = totalFileTarget.Concat(targetDirInfo.EnumerateFiles("*", SearchOption.AllDirectories));
                    //targetFileSize += targetDirInfo.EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);
                }

                // Set other var
                string stateStr = state == 0 ? "Inactive" : state == 1 ? "Active" : "Stopped";
                long totalFileSize = totalFileSource.Sum(file => file.Length);
                long totalFilesSizeCopied = totalFileTarget.Sum(file => file.Length);
                long totalFilesToCopy = totalFileSource.Count();
                long totalFilesRemains = totalFileSize - totalFilesSizeCopied;
                int percentDone = (int)((totalFilesSizeCopied / totalFileSize)*100);

                StatusLog statusLogModel = new StatusLog
                {
                    JobName = job.Name,
                    Horodatage = DateTime.Now.ToString("dd/mm/yyyy HH:mm:ss"),
                    State = stateStr,
                    TotalFilesToCopy = totalFilesToCopy,
                    TotalSizeToCopy = totalFileSize,
                    Progression = new Progression(percentDone, totalFilesRemains, currentSourcePath, currentTargetPath)
                };


                // Add or update the status of the job
                List<StatusLog> statutList = JsonConvert.DeserializeObject<List<StatusLog>>(File.ReadAllText(logPath))
                    ?? new List<StatusLog>();

                if (statutList.Exists(x => x.JobName == job.Name))
                {
                    int index = statutList.FindIndex(x => x.JobName == job.Name);
                    statutList[index] = statusLogModel;
                }
                else
                {
                    statutList.Add(statusLogModel);
                }

                File.WriteAllText(logPath, JsonSerializer.Serialize(statutList));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
