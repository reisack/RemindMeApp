using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RemindMe.Core.Localization
{
    public static class LocalizationManager
    {
        public static string GetString(string key)
        {
            if (CultureInfo.CurrentCulture.Name == "en-US")
            {
                return en_US.ResourceManager.GetString(key);
            }
            else
            {
                return fr_FR.ResourceManager.GetString(key);
            }
        }
    }
}
