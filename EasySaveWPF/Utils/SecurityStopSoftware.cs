using EasySave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasySave.utils
{
    internal class SecurityStopSoftware
    {
        public static bool BusinessSoftLunched()
        {
            string softname = Config.ReadSetting("businessSoft");
            return Process.GetProcesses().Any(p => p.ProcessName.Contains(softname));
        }
    }
}
