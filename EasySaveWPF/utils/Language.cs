using EasySave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Windows;

namespace EasySave.utils
{
    internal static class Language // Full static class to manage the language of the program
    {
        public static void SetLangue(string language)
        {
            Config.AddUpdateAppSettings("Language", language);

            Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(
                new ResourceDictionary()
                {
                    Source = new Uri("/Resources/Dictionary-" + language + ".xaml", UriKind.Relative)
                });
        }
    }
}
