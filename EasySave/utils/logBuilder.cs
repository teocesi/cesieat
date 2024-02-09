using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using EasySave.model;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace EasySave.utils
{
    internal class LogBuilder
    {
        // Build the path of the log file with the current date
        private static string BuildPathLog()
        {
            return Config.ReadSetting("LogPath") + "/logs/" + DateTime.Now.ToString("yyyy-MM-dd") +
                (Config.ReadSetting("LogType") == "json" ? ".json" : ".xml");
        }
        // Write the log in the history file (Append or create the file if it doesn't exist)
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
        // Check if the log file exists
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

            if (Config.ReadSetting("LogType") == "json")
            {
                logJson(executeLog);
            }
            else
            {
                logXML(executeLog);
            }
        }

        private static void logJson(HistoryLog executeLog)
        {
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

                    WriteLog(",\n" + JsonConvert.SerializeObject(executeLog, Newtonsoft.Json.Formatting.Indented) + "]");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                WriteLog("[" + JsonConvert.SerializeObject(executeLog, Newtonsoft.Json.Formatting.Indented) + "]");
            }
        }

        private static void logXML(HistoryLog executeLog)
        {
            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(executeLog.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = IsLogFileExiste()
            ;

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, executeLog, emptyNamespaces);
                WriteLog(stream.ToString());
            }
        }

        // Update the real time status of the job in the status log file
        public static void UpdateStatusLog(Job job, int state, string currentSourcePath, string currentTargetPath)
        {
            try
            {
                // Set log path file
                string logPath = Config.ReadSetting("LogPath") + "/logs/status." + Config.ReadSetting("LogType");
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

                    var targetPath = job.DestinationPath + "\\" + new DirectoryInfo(sourcePathTemp).Name;
                    DirectoryInfo targetDirInfo = new DirectoryInfo(targetPath);
                    totalFileTarget = totalFileTarget.Concat(targetDirInfo.EnumerateFiles("*", SearchOption.AllDirectories));
                }

                // Set other var
                string stateStr = state == 0 ? "Inactive" : state == 1 ? "Active" : "Stopped";
                long totalFileSize = totalFileSource.Sum(file => file.Length);
                long totalFilesSizeCopied = totalFileTarget.Sum(file => file.Length);
                long totalFilesSizeRemains = totalFileSize - totalFilesSizeCopied;

                long totalFilesToCopy = totalFileSource.Count();
                long FilesCopied = totalFileTarget.Count();
                long FilesRemains = totalFilesToCopy - FilesCopied;
                int percentDone = (int)((totalFilesSizeCopied * 100) / totalFileSize);

                StatusLog statusLogModel = new StatusLog
                {
                    JobName = job.Name,
                    Horodatage = DateTime.Now.ToString("dd/mm/yyyy HH:mm:ss"),
                    State = stateStr,
                    TotalFilesToCopy = totalFilesToCopy,
                    TotalSizeToCopy = totalFileSize,
                    Progression = new Progression(percentDone, totalFilesSizeRemains, FilesRemains, currentSourcePath, currentTargetPath)
                };


                //// Add or update the status of the job
                
                // Get the status log list
                List<StatusLog> statutList = new List<StatusLog>();
                if (Config.ReadSetting("LogType") == "json")
                {
                    statutList = JsonConvert.DeserializeObject<List<StatusLog>>(File.ReadAllText(logPath))
                    ?? new List<StatusLog>();
                }
                else
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<StatusLog>));
                    StringReader stringReader = new StringReader(File.ReadAllText(logPath));
                    statutList = (List<StatusLog>)xmlSerializer.Deserialize(stringReader);
                }

                // Add or update the status of the job
                if (statutList.Exists(x => x.JobName == job.Name))
                {
                    int index = statutList.FindIndex(x => x.JobName == job.Name);
                    statutList[index] = statusLogModel;
                }
                else
                {
                    statutList.Add(statusLogModel);
                }

                // Write the status log list in the file
                if (Config.ReadSetting("LogType") == "json")
                {
                    File.WriteAllText(logPath, JsonConvert.SerializeObject(statutList, Newtonsoft.Json.Formatting.Indented));
                }
                else
                {
                    var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    var serializer = new XmlSerializer(statutList.GetType());
                    var settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.OmitXmlDeclaration = IsLogFileExiste();

                    using (var stream = new StringWriter())
                    using (var writer = XmlWriter.Create(stream, settings))
                    {
                        serializer.Serialize(writer, statutList, emptyNamespaces);
                        File.WriteAllText(logPath, stream.ToString().ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
