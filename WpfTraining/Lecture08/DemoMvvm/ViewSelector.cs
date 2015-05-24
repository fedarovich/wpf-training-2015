using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using Microsoft.Practices.ServiceLocation;

namespace DemoMvvm
{
    public class ViewSelector : DataTemplateSelector, IValueConverter
    {
        private const string XamlTemplate = @"<DataTemplate DataType=""{{x:Type vm:{0}}}""><v:{1} /></DataTemplate>";

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return GetDataTemplate(item);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetDataTemplate(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static DataTemplate GetDataTemplate(object item)
        {
            if (item == null)
                return null;

            var viewModelType = item.GetType();

            // Note: Caching should be added here.

            var resolver = ServiceLocator.Current.GetInstance<IViewResolver>();
            var viewType = resolver.Resolve(viewModelType);
            if (viewType == null)
                return null;

            string xaml = string.Format(XamlTemplate, viewModelType.Name, viewType.Name);

            var context = new ParserContext { XamlTypeMapper = new XamlTypeMapper(new string[0]) };
            context.XamlTypeMapper.AddMappingProcessingInstruction(
                "vm", viewModelType.Namespace, viewModelType.Assembly.FullName);
            context.XamlTypeMapper.AddMappingProcessingInstruction(
                "v", viewType.Namespace, viewType.Assembly.FullName);
            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("vm", "vm");
            context.XmlnsDictionary.Add("v", "v");

            var template = (DataTemplate)XamlReader.Parse(xaml, context);
            return template;
        }
    }
}
