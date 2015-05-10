using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace ChangeNotifications.ViewModels
{
    public static class StaticPropertyChangedSampleViewModel
    {
        static StaticPropertyChangedSampleViewModel()
        {
            IncreaseValue = new RelayCommand(() => ClickCount += 1);
        }

        public static ICommand IncreaseValue { get; private set; }
        
        public static int ClickCount
        {
            get { return clickCount; }
            set
            {
                if (clickCount == value)
                    return;
                
                clickCount = value;
                OnStaticPropertyChanged();
            }
        }

        private static int clickCount;

        private static void OnStaticPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = StaticPropertyChanged;
            if (handler != null)
            {
                handler(null, new PropertyChangedEventArgs(propertyName));
            }
        }

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
    }
}
