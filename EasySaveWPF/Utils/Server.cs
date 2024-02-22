using EasySave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasySaveWPF.Utils
{
    // Command list : [list] <string> {optional}
    // help {command}
    // job run <job_name>
    // job stop <job_name>
    // job pause <job_name>
    // job status <job_name>
    // get [jobs|options]
    // option set <option_name> <option_value>
    // option get <option_name>

    internal class Server
    {
        private IPEndPoint localEndPoint;
        private Socket listener;

        Server()
        {
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];

            localEndPoint = new IPEndPoint(ipAddr, 11000);
            listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }
        public static void StartServer()
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
                    string data = null;
                    while (true)
                    {
                        int numByte = clientSocket.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, numByte);
                        if (data.IndexOf("<EOF>") > -1)
                            break;
                    }

                    byte[] message = Encoding.ASCII.GetBytes(" %"); // To be replaced by the command result

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
    }
}
