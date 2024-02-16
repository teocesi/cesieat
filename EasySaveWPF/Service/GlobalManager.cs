using EasySave.model;
using EasySave.View;
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
        private void home_option_button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new ConfigView();
        }

        private void home_create_button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new JobCreationView();
        }

        private void home_jobList_listViewChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                DataContext = new JobManagerView(new DUpdateViewJobList(UpdateViewJobList), item.ToString());
            }
        }

        public delegate void DUpdateViewJobList();

        public void UpdateViewJobList()
        {
            home_jobList_listView.ItemsSource = JobList.GetJobNames();
        }
    }
}
