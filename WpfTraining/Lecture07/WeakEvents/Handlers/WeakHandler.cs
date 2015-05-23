using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using JetBrains.Annotations;

namespace WeakEvents.Handlers
{
    public class WeakHandler : HandlerBase, IWeakEventListener
    {
        private static int instanceCount;

        public WeakHandler()
        {
            Interlocked.Increment(ref instanceCount);
            RaiseStaticPropertyChanged("InstanceCount");
        }

        ~WeakHandler()
        {
            Interlocked.Decrement(ref instanceCount);
            RaiseStaticPropertyChanged("InstanceCount");
        }

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(ClickEventManager))
            {
                OnClick(sender, (RoutedEventArgs)e);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void OnClick(object sender, RoutedEventArgs eventArgs)
        {
            Notify();
        }

        public static int InstanceCount
        {
            get { return Interlocked.CompareExchange(ref instanceCount, 0, 0); }
        }

        public override string ToString()
        {
            return "Weak Handler";
        }

        #region Static Property Changed

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected static void RaiseStaticPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = StaticPropertyChanged;
            if (handler != null)
                handler(null, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
