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

namespace EasySave
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Config config = Config.GetInstance();
            InitializeComponent();
            UpdateViewJobList();

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

            Thread.Sleep(1000);
        }
    }
}