using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageManager
{
    internal partial class PageManager
    {
        public static void ShowHomePage()
        {
            Console.Clear();
            Console.WriteLine("1. Create a new job");
            Console.WriteLine("2. Delete a job");
            Console.WriteLine("3. Launch a job");
            Console.WriteLine("4. Options");
            Console.WriteLine("5. Exit");

            Console.WriteLine("\nEnter your choice: ");
            Char choice = Console.ReadKey().KeyChar;

            switch (choice)
            {
                case '1':
                    ShowCreateJobPage();
                    break;
                case '2':
                    ShowDeleteJobPage();
                    break;
                case '3':
                    ShowRunJobPage();
                    break;
                case '4':
                    ShowOptionsPage();
                    break;
                case '5':
                    Environment.Exit(0);
                    break;
                default:
                    ShowHomePage();
                    break;
            }
        }


    }
}
