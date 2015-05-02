using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Validation.ViewModels
{
    public class ExceptionSampleViewModel : INotifyPropertyChanged
    {
        private int value;

        public int Value
        {
            get { return value; }
            set
            {
                if (value == this.value) return;

                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException("value", "The value must be in range [0, 100].");

                this.value = value;
                OnPropertyChanged();
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
