using EasySave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EasySave.model;

namespace EasySaveWPF.Utils
{
    internal class Server // Singleton
    {
        private static Server instance;
        private IPEndPoint localEndPoint;
        private Socket listener;

        public static Server GetInstance()
        {
            if (instance == null)
            {
                instance = new Server();
            }
            return instance;
        }

        private Server()
        {
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];

            localEndPoint = new IPEndPoint(ipAddr, 11000);
            listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public void StartServer()
        {
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11000);
            Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(5);
                while (true)
                {
                    Socket clientSocket = listener.Accept();
                    byte[] bytes = new Byte[1024];
                    string? data = null;
                    while (true)
                    {
                        int numByte = clientSocket.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, numByte);
                        if (data.IndexOf("<EOF>") > -1)
                            break;
                    }

                    //Remove <EOF> from the string
                    data = data[..^5];

                    byte[] message = Encoding.ASCII.GetBytes(CommandInterpreter(data));
                    clientSocket.Send(message);

                    // Close socket
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                listener.Close();
            }
        }


        // -------------------- Command interpreter --------------------
        // Command list : [choice list] <string> {optional}
        // help {command}
        // job run <job_name>
        // job stop <job_name>
        // job pause <job_name>
        // job status <job_name>
        // get [jobs|options]
        // option set <option_name> <option_value>
        // option get <option_name>
        // -------------------------------------------------------------
        private string CommandInterpreter(string command)
        {
            string[] parsedCommand = command.Split(' ');

            switch (parsedCommand[0])
            {
                case "help":
                    return HelpCommand(parsedCommand);
                case "job":
                    return JobCommand(parsedCommand);
                case "get":
                    return GetCommand(parsedCommand);
                case "option":
                    return OptionCommand(parsedCommand);
                default:
                    return "Unknown command";
            }
        }

        private string HelpCommand(string[] parsedCommand)
        {
            if (parsedCommand.Length == 1)
            {
                return "help {command}\njob run <job_name>\njob stop <job_name>\njob pause <job_name>\njob status <job_name>\nget [jobs|options]\noption set <option_name> <option_value>\noption get <option_name>";
            }
            else
            {
                switch (parsedCommand[1])
                {
                    case "help":
                        return "help {command}";
                    case "job":
                        return "job run <job_name>\njob stop <job_name>\njob pause <job_name>\njob status <job_name>";
                    case "get":
                        return "get [jobs|options]";
                    case "option":
                        return "option set <option_name> <option_value>\noption get <option_name>";
                    default:
                        return "Unknown command";
                }
            }
        }

        private string JobCommand(string[] parsedCommand)
        {
            if (parsedCommand.Length < 3)
            {
                return "Missing argument";
            }
            else
            {
                Job currentJob = JobList.getJobByName(parsedCommand[2]);
                if (currentJob == null)
                {
                    return "Job not found";
                }
                else
                {
                    switch (parsedCommand[1])
                    {
                        case "run":
                            Thread thread = new Thread(() => currentJob.Run());
                            thread.Start();
                            currentJob.State = 1;
                            return currentJob.Name + " running...";
                        case "stop":
                            currentJob.State = 0;
                            return "Job stopped.";
                        case "pause":
                            currentJob.State = 2;
                            return "Job paused.";
                        case "status":
                            return currentJob.Name + " status: " + (currentJob.State == 0 ? "Inactive" : currentJob.State == 1 ? "Active" : "Paused");
                        default:
                            return "Unknown command";
                    }
                }
            }
        }

        private string GetCommand(string[] parsedCommand)
        {
            if (parsedCommand.Length < 2)
            {
                return "Missing argument";
            }
            else
            {
                switch (parsedCommand[1])
                {
                    case "jobs":
                        IEnumerable<string> jobNames = JobList.GetJobNames();
                        return jobNames.Aggregate((i, j) => i + "\n" + j);
                    case "options":
                        return Config.ReadAllSettings();
                    default:
                        return "Unknown command";
                }
            }
        }

        private string OptionCommand(string[] parsedCommand)
        {
            if (parsedCommand.Length < 3)
            {
                return "Missing argument";
            }
            else
            {
                IEnumerable<string> optionList = Config.GetOptionNames();
                switch (parsedCommand[1])
                {
                    case "set":
                        if (parsedCommand.Length < 4)
                        {
                            return "Missing argument";
                        }
                        else
                        {
                            if (!optionList.Contains(parsedCommand[2]))
                            {
                                return $"Option {parsedCommand[2]} not found";
                            }
                            Config.AddUpdateAppSettings(parsedCommand[2], parsedCommand[3]);
                            return "Option set";
                        }
                    case "get":
                        if (!optionList.Contains(parsedCommand[2]))
                        {
                            return $"Option {parsedCommand[2]} not found";
                        }
                        return Config.ReadSetting(parsedCommand[2]);
                    default:
                        return "Unknown command";
                }
            }
        }
    }
}
