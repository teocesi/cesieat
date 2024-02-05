using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.model
{
    internal class StatusLog
    {
        public string JobName { get; set; }
        public string Horodatage { get; set; }
        public string State { get; set; }
        public int TotalFilesToCopy { get; set; }
        public int TotalSizeToCopy { get; set; }
        public string[] Progression { get; set; } // Nb left, Size left, Path source, Path target
    }
}
