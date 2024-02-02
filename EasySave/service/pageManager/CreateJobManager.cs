using EasySave;
using EasySave.utils;
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
                Console.WriteLine(Language.GetText("job_name"));
                name = Regex.Replace(Console.ReadLine(), "[|>]", string.Empty);
            }

            List<String> sourcePaths = new List<String>();
            Console.WriteLine($"\n{Language.GetText("source_path")} {Language.GetText("source_path_info")}");

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
                Console.WriteLine($"\n{Language.GetText("target_path")} ");
                targetPath = Console.ReadLine();

                if (targetPath != "" && !sourcePaths.Exists(sourcePath => targetPath.Contains(sourcePath)))
                {
                    break;
                }
                else
                {
                    Console.WriteLine(Language.GetText("target_not_empty"));
                }
            }

            bool isDifferential;
            Console.WriteLine(Language.GetText("copy_type"));
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
                Console.WriteLine(Language.GetText("unacceptable_ans"));
            }

            int priority;
            Console.WriteLine(Language.GetText("priority_ask"));
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
                Console.WriteLine(Language.GetText("unacceptable_ans"));
            }

            Model.SavingJob savingJob = new Model.SavingJob(name, sourcePaths, targetPath, isDifferential, priority, 0);

            Console.WriteLine(Language.GetText("job_created"));

            //Console.WriteLine("\nName: " + savingJob.GetName());
            //Console.WriteLine("Source path: " + String.Join(Environment.NewLine, savingJob.GetSourcePaths()));
            //Console.WriteLine("Target path: " + savingJob.GetDestinationPath());
            //Console.WriteLine("Type: " + savingJob.GetIsDifferential());
            //Console.WriteLine("Priority: " + savingJob.GetPriority());
            //Console.WriteLine("State: " + savingJob.GetState());

            Console.WriteLine(Language.GetText("key_continue"));
            Console.ReadKey();

            Config.AddJobIntoConfig(savingJob);
            ShowHomePage();
        }
    }
}
