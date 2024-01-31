using EasySave;
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
        public static void ShowCreateJobPage()
        {
            Console.Clear();
            string name = "";
            while(name.Length < 1)
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

            Console.WriteLine("\nTarget path: ");
            String targetPath = Console.ReadLine();

            int type;
            Console.WriteLine("\nType:\n1) Full 2) Differential");
            while (true)
            {
                Char type_char = Console.ReadKey().KeyChar;
                if (type_char == '1')
                {
                    type = 0;
                    break;
                }
                else if (type_char == '2')
                {
                    type = 1;
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

            Model.SavingJob savingJob = new Model.SavingJob(name, sourcePaths, targetPath, type, priority, 0);

            Console.WriteLine("\nJob created successfully.");

            Console.WriteLine("\nName: " + savingJob.Name);
            Console.WriteLine("Source path: " + String.Join(Environment.NewLine, savingJob.SourcePaths));
            Console.WriteLine("Target path: " + savingJob.DestinationPath);
            Console.WriteLine("Type: " + savingJob.Type);
            Console.WriteLine("Priority: " + savingJob.Priority);
            Console.WriteLine("State: " + savingJob.State);

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();

            Config.AddJobIntoConfig(savingJob);
            ShowHomePage();
        }
    }
}
