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
        public long FileLeft { get; set; }
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }

        public Progression(int percentDone, long sizeLeft, long fileLeft, string sourcePath, string targetPath)
        {
            this.PercentDone = percentDone;
            this.SizeLeft = sizeLeft;
            this.FileLeft = fileLeft;
            this.SourcePath = sourcePath;
            this.TargetPath = targetPath;
        }
    }
}
