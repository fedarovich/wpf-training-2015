using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using JetBrains.Annotations;

namespace ChangeNotifications.ViewModels
{
    /// <summary>
    /// IMPORTANT: This approach, though supported, is not recommended
    /// because it can cause memory leak under certain circumstances.
    /// </summary>
    public class InstanceEventSampleViewModel
    {
        public InstanceEventSampleViewModel()
        {
            IncreaseValue = new RelayCommand(() => ClickCount += 1);
        }

        public ICommand IncreaseValue { get; private set; }

        public int ClickCount
        {
            get { return clickCount; }
            set
            {
                if (clickCount == value)
                    return;

                clickCount = value;
                ClickCountChanged(this, EventArgs.Empty);
            }
        }

        private int clickCount;

        public event EventHandler ClickCountChanged = delegate {};
    }
}
