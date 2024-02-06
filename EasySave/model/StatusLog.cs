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
        public long TotalFilesToCopy { get; set; }
        public long TotalSizeToCopy { get; set; }
        public Progression Progression { get; set; }
    }

    internal class Progression
    {
        public int PercentDone { get; set; }
        public long SizeLeft { get; set; }
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }

        public Progression(int percentDone, long SizeLeft, string SourcePath, string TargetPath)
        {
            this.PercentDone = percentDone;
            this.SizeLeft = SizeLeft;
            this.SourcePath = SourcePath;
            this.TargetPath = TargetPath;
        }
    }
}
