using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace CustomMarkupExtensions.ViewModels
{
    public class EventToCommandViewModel : INotifyPropertyChanged
    {
        public EventToCommandViewModel()
        {
            HelloCommand = new RelayCommand(() => MessageBox.Show("Hello!"));
        }

        public ICommand HelloCommand { get; private set; }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
