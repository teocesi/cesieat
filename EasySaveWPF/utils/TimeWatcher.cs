using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EasySave.utils
{
    // Class to measure the time of a process, used to display the time of a backup
    // Instantiate the class at the beginning of the process and call the Stop method at the end
    internal class TimeWatcher
    {
        Stopwatch stopWatch = new Stopwatch();
        public TimeWatcher()
        {
            stopWatch.Start();
        }
        public string Stop()
        {
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            return ts.TotalMilliseconds.ToString() + " ms";
        }
    }
}
