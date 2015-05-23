using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using JetBrains.Annotations;

namespace WeakEvents.Handlers
{
    public abstract class HandlerBase : INotifyPropertyChanged
    {
        private bool notified;

        public bool Notified
        {
            get { return notified; }
            private set
            {
                if (value.Equals(notified)) return;
                notified = value;
                RaisePropertyChanged();
            }
        }

        protected async void Notify()
        {
            Notified = true;
            await Task.Delay(500);
            Notified = false;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
