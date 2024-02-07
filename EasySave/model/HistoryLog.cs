using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.model
{
    internal class HistoryLog
    {
        public string JobName { get; set; }
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
        public string Size { get; set; }
        public string TimeTransfer { get; set; }
        public string Horodatage { get; set; }
    }
}
