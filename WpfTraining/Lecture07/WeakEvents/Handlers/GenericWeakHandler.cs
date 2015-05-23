using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using JetBrains.Annotations;

namespace WeakEvents.Handlers
{
    public class GenericWeakHandler : HandlerBase
    {
        private static int instanceCount;

        public GenericWeakHandler()
        {
            Interlocked.Increment(ref instanceCount);
            RaiseStaticPropertyChanged("InstanceCount");
        }

        ~GenericWeakHandler()
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
            return "Generic Weak Handler";
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
