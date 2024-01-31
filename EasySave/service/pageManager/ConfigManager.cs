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
        public static void ShowOptionsPage()
        {
            Console.Clear();
            Console.WriteLine("1. Change language");
            Console.WriteLine("2. Change log path");
            Console.WriteLine("3. Reset jobs config");
            Console.WriteLine("4. Exit");
            Console.WriteLine("\nEnter your choice: ");

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
            Console.WriteLine("1. English");
            Console.WriteLine("2. French");
            Console.WriteLine("\nEnter your choice: ");

            Char choice = Console.ReadKey().KeyChar;

            switch (choice)
            {
                case '1':
                    Config.AddUpdateAppSettings("language", "EN");
                    break;
                case '2':
                    Config.AddUpdateAppSettings("language", "FR");
                    break;
            }
            ShowOptionsPage();
        }

        private static void ShowChangeLogPathPage()
        {
            Console.Clear();
            Console.WriteLine("Enter a new log path: ");
            string path = Console.ReadLine();
            Config.AddUpdateAppSettings("logPath", path);
            ShowOptionsPage();
        }

        private static void ShowResetJobsConfigPage()
        {
            Console.Clear();
            Console.WriteLine("Are you sure you want to reset jobs config? (y/n)");
            Char choice = Console.ReadKey().KeyChar;

            if (choice == 'y')
            {
                Config.AddUpdateAppSettings("JobList", "");
                Console.WriteLine("\nJobs config reseted. Press any key to return continue...");
                Console.ReadKey();
            }
            ShowOptionsPage();
        }
    }
}
