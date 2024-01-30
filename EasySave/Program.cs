using EasySave.Properties;
using Model;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasySave
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Config settings = new Config();
            ////SavingJob savingJob = new SavingJob("test", new List<String> { "test1", "test" }, "C:\\Users\\selya\\Documents\\CESI\\A3", 0, 0, 0);

            //Config.ReadAllSettings();

            //foreach (SavingJob savingJob in Config.GetSavingJobs())
            //{
            //    Console.WriteLine(savingJob.GetName());
            //}

            //Console.ReadKey();


            PageManager.PageManager.ShowHomeSelection();

            //Console.ReadKey();

            //// Get rdir
            //Console.WriteLine("Enter path");
            //String path = Console.ReadLine(); //@"C:\Users\selya\Documents\CESI\A3";

            //FileExplorer fileExplorer = new FileExplorer(path);


            //// End stop
            //Console.WriteLine("Press key to stop...");
            //Console.ReadKey();
        }
    }
}
