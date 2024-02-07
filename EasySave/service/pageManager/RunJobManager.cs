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
        public static void ShowRunJobPage()
        {
            Console.Clear();
            JobList.ShowJobList();

            Console.WriteLine(Language.GetText("select_job_to_run"));
            string input = Console.ReadLine().ToString();

            List<int> selectedJobs = new List<int>();
            for (int i = 0; i < input.Length; i++)
            {
                if (checkValidity(input[i], JobList.GetJobList().Count))
                {
                    int index = int.Parse(input[i].ToString());
                    if (!selectedJobs.Contains(index))
                    {
                        selectedJobs.Add(index);
                    }
                }
            }
            foreach (int index in selectedJobs)
            {
                JobList.GetJobList()[index].Run();
            }

            Console.WriteLine(Language.GetText("key_continue"));
            Console.ReadKey();
            ShowHomePage();
        }

        private static bool checkValidity(char inputChar, int max)
        {
            if (int.TryParse(inputChar.ToString(), out int inputInt) && (inputInt < max && inputInt >= 0))
            {
                return true;
            }
            return false;
        }
    }
}
