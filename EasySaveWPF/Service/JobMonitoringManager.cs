using EasySave.model;
using EasySave.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EasySave.View
{
    public partial class JobManagerView : UserControl
    {
        private void jobManager_start_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentJob.State == 0)
                {
                    Thread jobThread = new Thread(CurrentJob.Run);
                    jobThread.Start();
                }
                else if (CurrentJob.State == 2)
                {
                    SetOnRunning();
                }
                else if (CurrentJob.State == 1)
                {
                    if (MessageBox.Show("Job already running, force start?", "Info", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        SetOnStop();
                        Thread.Sleep(1000);
                        Thread jobThread = new Thread(CurrentJob.Run);
                        jobThread.Start();
                    }
                }
                jobManager_progressbar.Foreground = System.Windows.Media.Brushes.Green;
            }
            catch
            {
                MessageBox.Show("No job found");
            }
        }

        private void jobManager_delete_button_Click(object sender, RoutedEventArgs e)
        {
            SetOnStop();
            JobList.RemoveJob(CurrentJob.Name);
            this.updateViewJobListDelegate();
        }

        private void jobManager_pause_button_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentJob.State == 1) { SetOnPause(); } else { SetOnRunning(); };
        }

        private void jobManager_stop_button_Click(object sender, RoutedEventArgs e)
        {
            SetOnStop();
        }

        // Set the current job state
        private void SetOnRunning()
        {
            JobList.UpdateJobState(CurrentJob.Name, 1);
            jobManager_progressbar.Foreground = System.Windows.Media.Brushes.Green;
        }
        public void SetOnPause()
        {
            JobList.UpdateJobState(CurrentJob.Name, 2);
            jobManager_progressbar.Foreground = System.Windows.Media.Brushes.Yellow;
        }
        private void SetOnStop()
        {
            JobList.UpdateJobState(CurrentJob.Name, 0);
            LogBuilder.UpdateStatusLog(CurrentJob, "", "");
            jobManager_progressbar.Foreground = System.Windows.Media.Brushes.Red;
        }
    }
}
