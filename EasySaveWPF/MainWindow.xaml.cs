using EasySave.utils;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using EasySave.model;
using EasySave.View;
using System.ComponentModel;
using System.Diagnostics;
using EasySaveWPF.Utils;
using System.Threading;

namespace EasySave
{
    public partial class MainWindow : Window
    {
        Thread serverThread;
        public MainWindow()
        {
            Config config = Config.GetInstance();
            InitializeComponent();
            UpdateViewJobList();

            Server server = Server.GetInstance();
            serverThread = new Thread(server.StartServer);
            serverThread.Start();

            //tkt.Text = FindResource("Two").ToString();
            //tkt.SetResourceReference(TextBlock.TextProperty, "Two");
        }

        void ClosingWin(object sender, CancelEventArgs e)
        {
            foreach (Job job in JobList.jobList)
            {
                job.State = 0;
            }
            JobList.UpdateJobListConfig();

            Thread.Sleep(300);
            serverThread.Interrupt();
            Process.GetCurrentProcess().Kill();
        }
    }
}