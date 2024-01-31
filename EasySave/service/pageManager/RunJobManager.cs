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
        public static void ShowRunJobPage()
        {
            SavingJob[] jobs = Config.GetSavingJobs();
            SavingJob.ShowJobs(jobs);

            Console.WriteLine("\nSelect jobs to run then press space:");
            string input = Console.ReadLine().ToString();

            List<int> selectedJobs = new List<int>();
            for (int i = 0; i < input.Length; i++)
            {
                if (checkValidity(input[i], jobs.Length))
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
                jobs[index].Run();
            }

            Console.WriteLine("\nPress any key to return to the home page...");
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
