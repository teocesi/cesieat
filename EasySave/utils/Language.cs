using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.utils
{
    internal static class Language // Full static class to manage the language of the program
    {
        private static CultureInfo cul;
        public static ResourceManager resourceManager = new ResourceManager("EasySave.Resource.Res", typeof(Program).Assembly);

        public static void SetLangue(string language)
        {
            Config.AddUpdateAppSettings("Language", language);
            cul = CultureInfo.CreateSpecificCulture(language);
        }
        public static string GetText(string key)
        {
            return resourceManager.GetString(key, cul);
        }
    }
}
