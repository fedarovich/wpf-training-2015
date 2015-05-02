using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace ChangeNotifications.ViewModels
{
    public static class StaticEventSampleViewModel
    {
        static StaticEventSampleViewModel()
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
                ClickCountChanged(null, EventArgs.Empty);
            }
        }

        public static event EventHandler ClickCountChanged = delegate {};

        private static int clickCount;
    }
}
