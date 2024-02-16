using EasySave.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EasySave
{
    public partial class MainWindow : Window
    {
        private void UpdateViewJobList()
        {
            home_jobList_listView.ItemsSource = JobList.GetJobNames();
        }

        private void home_jobList_listViewChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                jobManager_jobName_textblock.Text = item.ToString();
            }
        }
    }
}
