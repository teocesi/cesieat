using EasySave.model;
using EasySave.utils;
using System;
using System.Collections.Generic;
using System.Linq;

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
                name = Console.ReadLine();

                List<string> names = JobList.jobList.Select(job => job.Name).ToList();
                if (names.Contains(name))
                {
                    Console.WriteLine(Language.GetText("job_name_exist"));
                    name = "";
                }
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
            Console.WriteLine("\n" + Language.GetText("copy_type"));
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

            byte priority;
            Console.WriteLine("\n" + Language.GetText("priority_ask"));
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

            Job savingJob = new Job(name, sourcePaths, targetPath, isDifferential, priority, 0);
            JobList.AddJob(savingJob);

            Console.WriteLine(Language.GetText("job_created"));
            Console.WriteLine(Language.GetText("key_continue"));
            Console.ReadKey();
            ShowHomePage();
        }
    }
}
