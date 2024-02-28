using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace EasySave.View
{
    public partial class ConfigView : UserControl
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+");
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
            option_koLimit_textBox.Text = Config.ReadSetting("koLimit");

            option_saveConfirm_textBlock.Text = "";
        }

        private void option_save_button_Click(object sender, RoutedEventArgs e)
        {
            // Check if all value are set
            if (option_koLimit_textBox.Text == "" || option_logPath_textBox.Text == "" || option_crypExt_textBox.Text == "" || option_businessSoft_textBox.Text == "" || option_cryptoPath_textBox.Text == "" || option_prioExt_textBox.Text == "" || option_cryptKey_textBox.Text == "")
            {
                option_saveConfirm_textBlock.Text = "All fields must be set";
                return;
            }

            if (IsTextAllowed(option_koLimit_textBox.Text))
            {
                Config.AddUpdateAppSettings("koLimit", option_koLimit_textBox.Text);
            }
            else
            {
                option_saveConfirm_textBlock.Text = "Invalid value for ko limit";
                return;
            }

            if (option_fr_radio.IsChecked == true)
            {
                Config.AddUpdateAppSettings("Language", "fr");
                utils.Language.SetLangue("fr");
            }
            else
            {
                Config.AddUpdateAppSettings("Language", "en");
                utils.Language.SetLangue("en");
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

        private static bool IsTextAllowed(string text)
        {
            return (!_regex.IsMatch(text));
        }
    }
}
