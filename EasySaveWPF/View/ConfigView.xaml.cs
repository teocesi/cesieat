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
    /// Logique d'interaction pour ConfigView.xaml
    /// </summary>
    public partial class ConfigView : UserControl
    {
        public ConfigView()
        {
            InitializeComponent();
            ShowOptionValue();
        }

        private void option_koLimit_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void option_fr_radio_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void option_koLimit_textBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
