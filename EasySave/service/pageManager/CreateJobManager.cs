using EasySave;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PageManager
{
    internal partial class PageManager
    {
        public static void ShowCreateJobPage()
        {
            Console.Clear();
            string name = "";
            while (name.Length < 1)
            {
                Console.WriteLine("Job name: ");
                name = Regex.Replace(Console.ReadLine(), "[|>]", string.Empty);
            }

            List<String> sourcePaths = new List<String>();
            Console.WriteLine("\nSource path: (Enter for each path | enter empty to valide)");

            while (true)
            {
                String path = Console.ReadLine();
                if (path == "")
                {
                    break;
                }
                sourcePaths.Add(path);
            }

            String targetPath = "";
            while (true)
            {
                Console.WriteLine("\nTarget path: ");
                targetPath = Console.ReadLine();

                if (targetPath != "" && !sourcePaths.Exists(sourcePath => targetPath.Contains(sourcePath)))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("\nTarget path can't be empty or in source path...");
                }
            }

            bool isDifferential;
            Console.WriteLine("\nType:\n1) Full 2) Differential");
            while (true)
            {
                Char type_char = Console.ReadKey().KeyChar;
                if (type_char == '1')
                {
                    isDifferential = false;
                    break;
                }
                else if (type_char == '2')
                {
                    isDifferential = true;
                    break;
                }
                Console.WriteLine("\nUnacceptable answer...");
            }

            int priority;
            Console.WriteLine("\nPriority: y/n");
            while (true)
            {
                Char priority_input = Console.ReadKey().KeyChar;
                if (priority_input == 'y')
                {
                    priority = 1;
                    break;
                }
                else if (priority_input == 'n')
                {
                    priority = 1;
                    break;
                }
                Console.WriteLine("\nUnacceptable answer...");
            }

            Model.SavingJob savingJob = new Model.SavingJob(name, sourcePaths, targetPath, isDifferential, priority, 0);

            Console.WriteLine("\nJob created successfully.");

            Console.WriteLine("\nName: " + savingJob.GetName());
            Console.WriteLine("Source path: " + String.Join(Environment.NewLine, savingJob.GetSourcePaths()));
            Console.WriteLine("Target path: " + savingJob.GetDestinationPath());
            Console.WriteLine("Type: " + savingJob.GetIsDifferential());
            Console.WriteLine("Priority: " + savingJob.GetPriority());
            Console.WriteLine("State: " + savingJob.GetState());

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();

            Config.AddJobIntoConfig(savingJob);
            ShowHomePage();
        }
    }
}
