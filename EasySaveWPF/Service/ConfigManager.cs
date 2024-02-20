using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EasySave.View
{
    public partial class ConfigView : UserControl
    {
        public void ShowOptionValue()
        {
            if (Config.ReadSetting("Language") == "fr")
            {
                option_fr_radio.IsChecked = true;
            }
            else
            {
                option_en_radio.IsChecked = true;
            }

            if (Config.ReadSetting("LogType") == "xml")
            {
                option_formatXML_radio.IsChecked = true;
            }
            else
            {
                option_formatJson_radio.IsChecked = true;
            }

            option_logPath_textBox.Text = Config.ReadSetting("logPath");
            option_crypExt_textBox.Text = Config.ReadSetting("cryptExt");
            option_businessSoft_textBox.Text = Config.ReadSetting("businessSoft");
            option_cryptoPath_textBox.Text = Config.ReadSetting("cryptExePath");
            option_prioExt_textBox.Text = Config.ReadSetting("priority");
            option_cryptKey_textBox.Text = Config.ReadSetting("cryptKey");

            option_saveConfirm_textBlock.Text = "";
        }

        private void option_save_button_Click(object sender, RoutedEventArgs e)
        {
            if (option_fr_radio.IsChecked == true)
            {
                Config.AddUpdateAppSettings("Language", "fr");
            }
            else
            {
                Config.AddUpdateAppSettings("Language", "en");
            }

            if (option_formatJson_radio.IsChecked == true)
            {
                Config.AddUpdateAppSettings("LogType", "json");
            }
            else
            {
                Config.AddUpdateAppSettings("LogType", "xml");
            }

            Config.AddUpdateAppSettings("logPath", option_logPath_textBox.Text);
            Config.AddUpdateAppSettings("cryptExt", option_crypExt_textBox.Text);
            Config.AddUpdateAppSettings("businessSoft", option_businessSoft_textBox.Text);
            Config.AddUpdateAppSettings("cryptExePath", option_cryptoPath_textBox.Text);
            Config.AddUpdateAppSettings("priority", option_prioExt_textBox.Text);
            Config.AddUpdateAppSettings("cryptKey", option_cryptKey_textBox.Text);

            option_saveConfirm_textBlock.Text = "Options saved";
        }
    }
}
