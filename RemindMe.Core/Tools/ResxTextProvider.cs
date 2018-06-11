using MvvmCross.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;

namespace RemindMe.Core.Tools
{
    public class ResxTextProvider : IMvxTextProvider
    {
        private readonly ResourceManager _resourceManager;

        public ResxTextProvider(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
            CurrentLanguage = new CultureInfo("en-US");
        }

        public CultureInfo CurrentLanguage { get; set; }

        public string GetText(string namespaceKey,
            string typeKey, string name)
        {
            string resolvedKey = name;

            if (!string.IsNullOrEmpty(typeKey))
            {
                resolvedKey = $"{typeKey}.{resolvedKey}";
            }

            if (!string.IsNullOrEmpty(namespaceKey))
            {
                resolvedKey = $"{namespaceKey}.{resolvedKey}";
            }

            return _resourceManager.GetString(resolvedKey, CurrentLanguage);
        }

        public string GetText(string namespaceKey, string typeKey, string name, params object[] formatArgs)
        {
            string baseText = GetText(namespaceKey, typeKey, name);

            if (string.IsNullOrEmpty(baseText))
            {
                return baseText;
            }

            return string.Format(baseText, formatArgs);
        }

        public bool TryGetText(out string textValue, string namespaceKey, string typeKey, string name)
        {
            bool textExists = true;
            try
            {
                textValue = GetText(namespaceKey, typeKey, name);
            }
            catch (Exception ex)
            {
                textValue = null;
                textExists = false;
            }

            if (string.IsNullOrEmpty(textValue))
            {
                textExists = false;
            }

            return textExists;
        }

        public bool TryGetText(out string textValue, string namespaceKey, string typeKey, string name, params object[] formatArgs)
        {
            bool textExists = true;
            try
            {
                var baseText = GetText(namespaceKey, typeKey, name);
                textValue = string.Format(baseText, formatArgs);
            }
            catch (Exception ex)
            {
                textValue = null;
                textExists = false;
            }

            if (string.IsNullOrEmpty(textValue))
            {
                textExists = false;
            }

            return textExists;
        }
    }
}
