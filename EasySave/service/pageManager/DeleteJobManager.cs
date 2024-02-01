using EasySave;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PageManager
{
    internal partial class PageManager
    {
        public static void ShowDeleteJobPage()
        {
            SavingJob[] jobs = Config.GetSavingJobs();
            SavingJob.ShowJobs(jobs);

            Console.WriteLine("\nSelect a job to delete or space to exit:");
            string input = Console.ReadKey().KeyChar.ToString();
            int index = -1;
            if (int.TryParse(input, out index))
            {
                if (index >= 0 && index < jobs.Length)
                {
                    Console.WriteLine($"\n{jobs[index].GetName()} deleted.");
                    Config.DeleteJobIntoConfig(jobs[index]);
                    Console.WriteLine("Press key to continue...");
                    Console.ReadKey();
                }
            }
            ShowHomePage();
        }
    }
}
