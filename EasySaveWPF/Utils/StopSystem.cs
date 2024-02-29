using EasySave;
using EasySave.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasySave.utils
{
    internal class StopCopySystem
    {
        public static bool BusinessSoftLunched()
        {
            List<string> softnames = Config.ReadSetting("businessSoft").Split("\r\n").ToList();
            var process = Process.GetProcesses();

            foreach (string softname in softnames)
            {
                if (process.Any(p => p.ProcessName.Contains(softname)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
