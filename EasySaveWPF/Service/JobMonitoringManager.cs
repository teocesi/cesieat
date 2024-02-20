using EasySave.model;
using EasySave.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

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
                    SetOnRunning();
                    Thread jobThread = new Thread(CurrentJob.Run);
                    jobThread.Start();
                }
                //else if (CurrentJob.State == 1)
                //{
                //    if (MessageBox.Show("Job already running, force start?", "Info", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                //    {
                //        SetOnStop();
                //        Thread.Sleep(1000);

                //        SetOnRunning();
                //        Thread jobThread = new Thread(CurrentJob.Run);
                //        jobThread.Start();
                //    }
                //}
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
            this.UpdateViewJobListDelegate();
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
            jobManager_jobState_textblock.SetResourceReference(TextBlock.TextProperty, "stateRunning");
            jobManager_progressbar.Foreground = System.Windows.Media.Brushes.Green;
            UpDateButtonState();
        }
        public void SetOnPause()
        {
            JobList.UpdateJobState(CurrentJob.Name, 2);
            jobManager_jobState_textblock.SetResourceReference(TextBlock.TextProperty, "statePaused");
            jobManager_progressbar.Foreground = System.Windows.Media.Brushes.Yellow;
            UpDateButtonState();
        }
        private void SetOnStop()
        {
            JobList.UpdateJobState(CurrentJob.Name, 0);
            LogBuilder.UpdateStatusLog(CurrentJob, "", "");
            jobManager_jobState_textblock.SetResourceReference(TextBlock.TextProperty, "stateInactive");
            jobManager_progressbar.Foreground = System.Windows.Media.Brushes.Red;
            UpDateButtonState();
        }

        // Update the job progress bar
        public void UpdateProgressBar(int progress)
        {
            if (Application.Current is null || Application.Current.Dispatcher is null)
                return;

            // Marshall to Main Thread
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, ()=> jobManager_progressbar.Value = progress);
            if (progress == 100)
            {
                UpDateButtonState();
            }
        }
        private void UpDateButtonState()
        {
            switch (CurrentJob.State)
            {
                case 0:
                    jobManager_start_button.IsEnabled = true;
                    jobManager_delete_button.IsEnabled = true;
                    jobManager_pause_button.IsEnabled = false;
                    jobManager_stop_button.IsEnabled = false;
                    break;
                case 1:
                case 2:
                    jobManager_start_button.IsEnabled = false;
                    jobManager_delete_button.IsEnabled = false;
                    jobManager_pause_button.IsEnabled = true;
                    jobManager_stop_button.IsEnabled = true;
                    break;
            }
        }
    }
}
