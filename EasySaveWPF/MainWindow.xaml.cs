using EasySave.utils;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;

namespace EasySave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            utils.Language.SetLangue(Config.ReadSetting("Language"));
            InitializeComponent();

            //tkt.Text = FindResource("Two").ToString();
            //tkt.SetResourceReference(TextBlock.TextProperty, "Two");
        }
    }
}