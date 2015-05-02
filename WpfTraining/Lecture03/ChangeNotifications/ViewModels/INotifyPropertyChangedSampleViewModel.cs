using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace ChangeNotifications.ViewModels
{
    public class INotifyPropertyChangedSampleViewModel : INotifyPropertyChanged
    {
        public INotifyPropertyChangedSampleViewModel()
        {
            IncreaseValue = new RelayCommand(() => ClickCount += 1);
        }

        public ICommand IncreaseValue { get; private set; }
        
        public int ClickCount
        {
            get { return clickCount; }
            set
            {
                if (value == clickCount) 
                    return;

                clickCount = value;
                OnPropertyChanged();
            }
        }

        private int clickCount;

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [JetBrains.Annotations.NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
