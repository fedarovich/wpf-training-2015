using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using JetBrains.Annotations;
using Microsoft.Expression.Interactivity.Core;

namespace BlendBehaviors.ViewModels
{
    public class EventTriggerDemoViewModel : INotifyPropertyChanged
    {
        public EventTriggerDemoViewModel()
        {
            SayHelloCommand = new ActionCommand(
                () => MessageBox.Show("Hello from view model command."));
        }

        public void SayHello()
        {
            MessageBox.Show("Hello from view model method.");
        }

        public ICommand SayHelloCommand { get; private set; }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
