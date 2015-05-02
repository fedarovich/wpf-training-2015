using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Validation.ViewModels
{
    public class NotifyDataErrorInfoSampleViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private int value;

        public int Value
        {
            get { return value; }
            set
            {
                if (value == this.value) return;
                this.value = value;
                OnPropertyChanged();

                // NOTE: In real application you will create a set of rules
                // that will be checked on property changed.
                // For example, see:
                // http://www.c-sharpcorner.com/UploadFile/tirthacs/inotifydataerrorinfo-in-wpf/
                // http://blog.micic.ch/net/easy-mvvm-example-with-inotifypropertychanged-and-inotifydataerrorinfo
                ValidateValue(value);
            }
        }

        private async void ValidateValue(int value)
        {
            // Emulate asynchronous validation.
            await Task.Delay(1000);
            if (value < 0 || value > 100)
            {
                var propertyErrors = new[] {"The value must be in range [0, 100]"};
                errors["Value"] = propertyErrors;
            }
            else
            {
                errors.Remove("Value");
            }
            ErrorsChanged(this, new DataErrorsChangedEventArgs("Value"));
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region INotifyDataErrorInfo

        private readonly IDictionary<string, IList<string>> errors = new Dictionary<string, IList<string>>();

        public IEnumerable GetErrors(string propertyName)
        {
            IList<string> propertyErrors;
            return errors.TryGetValue(propertyName, out propertyErrors) ? propertyErrors : Enumerable.Empty<string>();
        }

        public bool HasErrors
        {
            get { return errors.SelectMany(x => x.Value).Any(); }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate {};

        #endregion
    }
}
