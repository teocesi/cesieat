using EasySave;
using EasySave.model;
using EasySave.utils;
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
            Console.Clear();
            JobList.ShowJobList();

            Console.WriteLine(Language.GetText("select_delete_job"));
            string input = Console.ReadKey().KeyChar.ToString();
            int index = -1;
            if (int.TryParse(input, out index))
            {
                if (index >= 0 && index < JobList.GetJobList().Count)
                {
                    Console.WriteLine($"\n{JobList.GetJobList()[index].GetName()} {Language.GetText("deleted")}");
                    JobList.RemoveJob(JobList.GetJobList()[index]);

                    Console.WriteLine(Language.GetText("key_continue"));
                    Console.ReadKey();
                }
            }
            ShowHomePage();
        }
    }
}
