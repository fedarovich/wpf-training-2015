using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UpdateSourceTriggers
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string propertyChangedText;
        private string lostFocusText;
        private string explicitText;

        public string PropertyChangedText
        {
            get { return propertyChangedText; }
            set
            {
                if (value == propertyChangedText) return;
                propertyChangedText = value;
                OnPropertyChanged();
            }
        }

        public string LostFocusText
        {
            get { return lostFocusText; }
            set
            {
                if (value == lostFocusText) return;
                lostFocusText = value;
                OnPropertyChanged();
            }
        }

        public string ExplicitText
        {
            get { return explicitText; }
            set
            {
                if (value == explicitText) return;
                explicitText = value;
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
