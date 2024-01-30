using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageManager
{
    internal partial class PageManager
    {
        public static void ShowHomeSelection()
        {
            Console.Clear();
            Console.WriteLine("1. Create a new job");
            Console.WriteLine("2. Modify a job");
            Console.WriteLine("3. Delete a job");
            Console.WriteLine("4. Launch a job");
            Console.WriteLine("5. Exit");

            Console.WriteLine("\nEnter your choice: ");
            Char choice = Console.ReadKey().KeyChar;

            switch (choice)
            {
                case '1':
                    ShowCreateJob();
                    break;
                case '2':
                    //showModifyJob();
                    break;
                case '3':
                    //showDeleteJob();
                    break;
                case '4':
                    //showLaunchJob();
                    break;
                case '5':
                    Environment.Exit(0);
                    break;
                default:
                    ShowHomeSelection();
                    break;
            }
        }

        
    }
}
