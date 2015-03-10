using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace Common
{
    public class CountryViewModel : INotifyPropertyChanged
    {
        private static readonly Lazy<IList<CountryViewModel>> countries = new Lazy<IList<CountryViewModel>>(LoadCountries);

        public CountryViewModel(XElement element)
        {
            CountryCode = (string) element.Attribute("countryCode");
            Name = (string) element.Attribute("countryName");
            CurrencyCode = (string) element.Attribute("currencyCode");
            Population = (long) element.Attribute("population");
            Capital = (string) element.Attribute("capital");
            Continent = (string) element.Attribute("continentName");
            Area = (double) element.Attribute("areaInSqKm");
            Languages = (string) element.Attribute("languages");
        }

        public string CountryCode { get; private set; }

        public string Name { get; private set; }

        public string CurrencyCode { get; private set; }

        public long Population { get; private set; }

        public string Capital { get; private set; }

        public string Continent { get; private set; }

        public double Area { get; private set; }

        public string Languages { get; private set; }

        public double PopulationDensity
        {
            get { return Population/Area; }
        }

        public static IEnumerable<CountryViewModel> AllCountries
        {
            get { return countries.Value; }
        }

        private static IList<CountryViewModel> LoadCountries()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("Common.Countries.xml"))
            using (var reader = new StreamReader(stream))
            {
                var xml = XElement.Load(reader);
                return xml.Elements().Select(x => new CountryViewModel(x)).ToArray();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
