using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.service.utils
{
    internal class logBuilder
    {
        public static string BuildLog(string logType, string logMessage)
        {
            string log = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - [" + logType + "] " + logMessage + "\n";
            return log;
        }
    }
}
