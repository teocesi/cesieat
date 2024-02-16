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

namespace EasySave.View
{
    public partial class JobManagerView : UserControl
    {
        MainWindow.DUpdateViewJobList updateViewJobListDelegate;

        public JobManagerView() { InitializeComponent(); }

        public JobManagerView(MainWindow.DUpdateViewJobList updateViewJobListDelegate, string jobName)
        {
            InitializeComponent();
            this.updateViewJobListDelegate = updateViewJobListDelegate;
            jobManager_jobName_textblock.Text = jobName;
        }
    }
}
