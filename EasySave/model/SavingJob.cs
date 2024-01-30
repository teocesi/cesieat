using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Model
{
    internal class SavingJob
    {
        private String name;
        private List<String> sourcePaths;
        private String destinationPath;
        private int type;
        private int priority;
        private int state;

        public SavingJob(String name, List<String> sourcePaths, String destinationPath, int type, int priority, int state)
        {
            this.name = name;
            this.sourcePaths = sourcePaths;
            this.destinationPath = destinationPath;
            this.type = type;
            this.priority = priority;
            this.state = state;
        }
        public SavingJob(String SavingJobStr)
        {
            string[] jobConfigTab = SavingJobStr.Split('|');

            try
            {
                this.name = jobConfigTab[0];
                this.sourcePaths = jobConfigTab[1].Split(',').ToList();
                this.destinationPath = jobConfigTab[2];
                this.type = Int32.Parse(jobConfigTab[3]);
                this.priority = Int32.Parse(jobConfigTab[4]);
                this.state = Int32.Parse(jobConfigTab[5]);
            }
            catch (Exception)
            {
                throw new Exception("Error while parsing SavingJob string");
            }
        }

        public void AddSourcePath(String sourcePath)
        {
            this.sourcePaths.Add(sourcePath);
        }

        public void RemoveSourcePath(String sourcePath)
        {
            this.sourcePaths.Remove(sourcePath);
        }

        public override String ToString()
        {
            return $"{this.name}|{string.Join(",", this.sourcePaths)}|{this.destinationPath}|{this.type}|{this.priority}|{this.state}";
        }

        // Getters and setters
        public String GetName()
        {
            return this.name;
        }
        public List<String> GetSourcePaths()
        {
            return this.sourcePaths;
        }
        public String GetDestinationPath()
        {
            return this.destinationPath;
        }
        public new int GetType()
        {
            return this.type;
        }
        public int GetPriority()
        {
            return this.priority;
        }
        public int GetState()
        {
            return this.state;
        }

        public void SetName(String name)
        {
            this.name = name;
        }
        public void SetSourcePaths(List<String> sourcePaths)
        {
            this.sourcePaths = sourcePaths;
        }
        public void SetDestinationPath(String destinationPath)
        {
            this.destinationPath = destinationPath;
        }
        public void SetType(int type)
        {
            this.type = type;
        }
        public void SetPriority(int priority)
        {
            this.priority = priority;
        }
        public void SetState(int state)
        {
            this.state = state;
        }
    }
}
