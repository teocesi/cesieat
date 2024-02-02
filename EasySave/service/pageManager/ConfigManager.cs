using EasySave;
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
        public static void ShowOptionsPage()
        {
            Console.Clear();
            Console.WriteLine(Language.GetText("change_language"));
            Console.WriteLine(Language.GetText("change_log_path"));
            Console.WriteLine(Language.GetText("reset_job_config"));
            Console.WriteLine($"4. {Language.GetText("exit")}");
            Console.WriteLine(Language.GetText("enter_choice"));

            Char choice = Console.ReadKey().KeyChar;

            switch (choice)
            {
                case '1':
                    ShowChangeLanguagePage();
                    break;
                case '2':
                    ShowChangeLogPathPage();
                    break;
                case '3':
                    ShowResetJobsConfigPage();
                    break;
                case '4':
                    ShowHomePage();
                    break;
                default:
                    ShowOptionsPage();
                    break;
            }
        }

        private static void ShowChangeLanguagePage()
        {
            Console.Clear();
            Console.WriteLine(Language.GetText("english"));
            Console.WriteLine(Language.GetText("french"));
            Console.WriteLine(Language.GetText("enter_choice"));

            Char choice = Console.ReadKey().KeyChar;

            switch (choice)
            {
                case '1':
                    Config.AddUpdateAppSettings("language", "en");
                    break;
                case '2':
                    Config.AddUpdateAppSettings("language", "fr");
                    break;
            }
            ShowOptionsPage();
        }

        private static void ShowChangeLogPathPage()
        {
            Console.Clear();
            Console.WriteLine(Language.GetText("enter_log_path"));
            string path = Console.ReadLine();
            Config.AddUpdateAppSettings("logPath", path);
            ShowOptionsPage();
        }

        private static void ShowResetJobsConfigPage()
        {
            Console.Clear();
            Console.WriteLine(Language.GetText("confirm_reset_job"));
            Char choice = Console.ReadKey().KeyChar;

            if (choice == 'y')
            {
                Config.AddUpdateAppSettings("JobList", "");
                Console.WriteLine(Language.GetText("job_reseted"));
                Console.ReadKey();
            }
            ShowOptionsPage();
        }
    }
}
