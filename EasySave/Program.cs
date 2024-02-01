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
using System.Globalization;
using System.Resources;

namespace EasySave
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Config config = Config.GetInstance();
            PageManager.PageManager.ShowHomePage();

            //Config.SetLangue("fr");
            //string text = Config.GetText("test");
            //Console.WriteLine(text);

            //Config.SetLangue("en");
            //text = Config.GetText("test");
            //Console.WriteLine(text);

            //Console.ReadLine();
        }
    }
}
