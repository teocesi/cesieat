using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// Logique d'interaction pour JobManagerView.xaml
    /// </summary>
    public partial class JobManagerView : UserControl
    {
        public JobManagerView() { InitializeComponent(); }

        public JobManagerView(string jobName)
        {
            InitializeComponent();
            jobManager_jobName_textblock.Text = jobName;
        }
    }
}
