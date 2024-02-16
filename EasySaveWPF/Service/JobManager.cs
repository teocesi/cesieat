using EasySave.model;
using EasySave.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasySave
{
    public partial class MainWindow : Window
    {
        private void jobManager_start_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Thread jobThread = new Thread(JobList.getJobByName(jobManager_jobName_textblock.Text).Run);
                jobThread.Start();
            }
            catch
            {
                MessageBox.Show("No job found");
            }
        }

        private void jobManager_delete_button_Click(object sender, RoutedEventArgs e)
        {
            JobList.RemoveJob(jobManager_jobName_textblock.Text);
            UpdateViewJobList();
        }
    }
}
