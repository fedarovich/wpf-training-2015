using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Validation.ViewModels
{
    public class DataErrorInfoSampleViewModel : INotifyPropertyChanged, IDataErrorInfo
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
                // For example, see http://www.codeproject.com/Tips/784331/WPF-MVVM-Validation-ViewModel-using-IDataErrorInfo
                ValidateValue(value);
            }
        }

        private void ValidateValue(int value)
        {
            if (value < 0 || value > 100)
            {
                errors["Value"] = "The value must be in range [0, 100]";
            }
            else
            {
                errors.Remove("Values");
            }
        }

        #region IDataErrorInfo

        private readonly IDictionary<string, string> errors = new Dictionary<string, string>(); 

        public string this[string columnName]
        {
            get
            {
                string error;
                return errors.TryGetValue(columnName, out error) ? error : string.Empty;
            }
        }

        public string Error { get; private set; }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
