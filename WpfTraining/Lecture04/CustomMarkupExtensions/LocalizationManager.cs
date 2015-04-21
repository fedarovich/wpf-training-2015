using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace CustomMarkupExtensions
{
    public class LocalizationManager : INotifyPropertyChanged
    {
        private static readonly LocalizationManager instance = new LocalizationManager();
        private CultureInfo currentUiCulture = CultureInfo.GetCultureInfo("en-US");

#region Constructors

        private LocalizationManager()
        {
        }

        static LocalizationManager()
        {
        }

#endregion

        public static LocalizationManager Instance
        {
            get { return instance; }
        }

        public IEnumerable<CultureInfo> Cultures
        {
            get
            {
                yield return CultureInfo.GetCultureInfo("en-US");
                yield return CultureInfo.GetCultureInfo("de-DE");
            }
        }

        public CultureInfo CurrentUICulture
        {
            get { return currentUiCulture; }
            set
            {
                currentUiCulture = value;
                OnPropertyChanged();
                OnPropertyChanged("Item[]");
            }
        }

        public string this[string key]
        {
            get { return Properties.Resources.ResourceManager.GetString(key, CurrentUICulture); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
