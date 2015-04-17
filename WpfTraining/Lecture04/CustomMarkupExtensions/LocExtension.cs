using System;
using System.Globalization;
using System.Windows.Markup;

namespace CustomMarkupExtensions
{
    [MarkupExtensionReturnType(typeof(string))]
    public class LocExtension : MarkupExtension
    {
        private readonly string key;

        public LocExtension(string key)
        {
            this.key = key;
        }

        public string Culture { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var culture = string.IsNullOrWhiteSpace(Culture) ? null : CultureInfo.GetCultureInfo(Culture);
            return Properties.Resources.ResourceManager.GetString(key, culture);
        }
    }
}
