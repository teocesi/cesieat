using EasySave;
using EasySave.utils;
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
            Console.Clear();
            SavingJob.ShowJobs(jobs);

            Console.WriteLine(Language.GetText("select_delete_job"));
            string input = Console.ReadKey().KeyChar.ToString();
            int index = -1;
            if (int.TryParse(input, out index))
            {
                if (index >= 0 && index < jobs.Length)
                {
                    Console.WriteLine($"\n{jobs[index].GetName()} {Language.GetText("deleted")}");
                    Config.DeleteJobIntoConfig(jobs[index]);
                    Console.WriteLine(Language.GetText("key_continue"));
                    Console.ReadKey();
                }
            }
            ShowHomePage();
        }
    }
}
