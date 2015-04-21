using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace CustomMarkupExtensions
{
    public class DynLocExtension : MarkupExtension
    {
        private readonly string key;

        public DynLocExtension(string key)
        {
            this.key = key;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var path = "[" + key + "]";
            var binding = new Binding(path)
            {
                Source = LocalizationManager.Instance,
                Mode = BindingMode.OneWay
            };
            return binding.ProvideValue(serviceProvider);
        }
    }
}
