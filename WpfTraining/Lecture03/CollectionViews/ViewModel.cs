using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using Common;
using JetBrains.Annotations;

namespace CollectionViews
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string filterText;
        private string sortProperty = "Name";
        private ListSortDirection sortDirection = ListSortDirection.Ascending;

        public ViewModel()
        {
            CountriesView = CollectionViewSource.GetDefaultView(CountryViewModel.AllCountries);
            CountriesView.Filter = FilterByName;
            RefreshSort();
            
            SortProperties = new[] {"Name", "Area", "Population", "PopulationDensity"};
            SortDirections = new[] {ListSortDirection.Ascending, ListSortDirection.Descending};
        }

        private bool FilterByName(object obj)
        {
            var country = obj as CountryViewModel;
            if (country == null)
                return false;

            if (string.IsNullOrWhiteSpace(FilterText))
                return true;

            return country.Name.StartsWith(FilterText.Trim(), StringComparison.CurrentCultureIgnoreCase);
        }

        public ICollectionView CountriesView { get; private set; }

        public IEnumerable<string> SortProperties { get; private set; }

        public IEnumerable<ListSortDirection> SortDirections { get; private set; }

        public string FilterText
        {
            get { return filterText; }
            set
            {
                if (value == filterText) return;
                filterText = value;
                OnPropertyChanged();

                // Refresh filter
                CountriesView.Refresh();
            }
        }

        public string SortProperty
        {
            get { return sortProperty; }
            set
            {
                if (value == sortProperty) return;
                sortProperty = value;
                OnPropertyChanged();

                RefreshSort();
            }
        }

        public ListSortDirection SortDirection
        {
            get { return sortDirection; }
            set
            {
                if (value == sortDirection) return;
                sortDirection = value;
                OnPropertyChanged();

                RefreshSort();
            }
        }

        private void RefreshSort()
        {
            using (CountriesView.DeferRefresh())
            {
                CountriesView.SortDescriptions.Clear();
                CountriesView.SortDescriptions.Add(new SortDescription(SortProperty, SortDirection));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
