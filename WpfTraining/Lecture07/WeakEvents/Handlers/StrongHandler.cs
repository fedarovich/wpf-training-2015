using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using JetBrains.Annotations;

namespace WeakEvents.Handlers
{
    public class StrongHandler : HandlerBase
    {
        private static int instanceCount;

        public StrongHandler()
        {
            Interlocked.Increment(ref instanceCount);
            RaiseStaticPropertyChanged("InstanceCount");
        }

        ~StrongHandler()
        {
            Interlocked.Decrement(ref instanceCount);
            RaiseStaticPropertyChanged("InstanceCount");
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
            return "Strong Handler";
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
