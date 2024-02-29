using EasySave.model;
using EasySave.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EasySave.View
{
    public partial class JobManagerView : UserControl
    {
        private Job CurrentJob;
        MainWindow.DUpdateViewJobList UpdateViewJobListDelegate;
        public delegate void DUpdateProgressBar(int value);

        public JobManagerView() { InitializeComponent(); }
        public JobManagerView(MainWindow.DUpdateViewJobList updateViewJobListDelegate, string jobName)
        {
            InitializeComponent();
            this.UpdateViewJobListDelegate = updateViewJobListDelegate;
            this.CurrentJob = JobList.getJobByName(jobName);
            LogBuilder.ProgressBarJob = CurrentJob;
            LogBuilder.UpdateProgressBarDelegate = new DUpdateProgressBar(UpdateProgressBar);
            jobManager_jobName_textblock.Text = jobName;

            switch (CurrentJob.State)
            {
                case 0:
                    SetOnStop();
                    break;
                case 1:
                    SetOnRunning();
                    break;
                case 2:
                    SetOnPause();
                    break;
            }
        }
    }
}
