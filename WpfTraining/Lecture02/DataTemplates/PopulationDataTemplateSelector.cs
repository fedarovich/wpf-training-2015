using System.Windows;
using System.Windows.Controls;
using Common;

namespace DataTemplates
{
    public class PopulationDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate HighPopulationDensityTemplate { get; set; }

        public DataTemplate NormalPopulationDensityTemplate { get; set; }

        public double HighDensityLimit { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var country = item as CountryViewModel;
            if (country != null)
            {
                if (country.PopulationDensity > HighDensityLimit)
                {
                    return HighPopulationDensityTemplate;
                }
                else
                {
                    return NormalPopulationDensityTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
