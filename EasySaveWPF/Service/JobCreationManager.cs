using EasySave.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EasySave.View
{
    public partial class JobCreationView : UserControl
    {
        private void jobCreation_create_button_Click(object sender, RoutedEventArgs e)
        {
            string jobName = jobCreation_jobName_textBox.Text;
            List<string> source = jobCreation_sourcePath_textBox.Text.Split("\r\n").ToList();
            string destination = jobCreation_targetPath_textBox.Text;
            bool IsDifferential = jobCreation_diff_checkBox.IsChecked.Value;
            bool priority = jobCreation_priority_checkBox.IsChecked.Value;
            byte state = 0;

            var names = JobList.GetJobNames();
            if (names.Contains(jobName))
            {
                jobCreation_message_textBlock.Text = FindResource("nameUsed").ToString();
                jobCreation_message_textBlock.Foreground = Brushes.Red;
                return;
            }
            if (jobName == "" || source.Count == 0 || source.Contains("") || destination == "")
            {
                jobCreation_message_textBlock.Text = FindResource("fillAll").ToString();
                jobCreation_message_textBlock.Foreground = Brushes.Red;
                return;
            }

            // Add job to the list
            JobList.AddJob(new Job(jobName, source, destination, IsDifferential, priority, state));
            this.updateViewJobListDelegate();

            jobCreation_message_textBlock.Text = FindResource("jobCreated").ToString();
            jobCreation_message_textBlock.Foreground = Brushes.Green;
        }
    }
}
