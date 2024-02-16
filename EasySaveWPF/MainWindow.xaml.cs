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
using EasySave.model;

namespace EasySave
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Config config = Config.GetInstance();
            InitializeComponent();
            UpdateViewJobList();

            //tkt.Text = FindResource("Two").ToString();
            //tkt.SetResourceReference(TextBlock.TextProperty, "Two");
        }

        private void home_option_button_Click(object sender, RoutedEventArgs e)
        {
            ShowOptionValue();
        }
    }
}