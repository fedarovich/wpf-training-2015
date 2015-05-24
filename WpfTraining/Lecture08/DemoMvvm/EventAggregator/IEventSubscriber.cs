using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMvvm
{
    public interface IEventSubscriber<in T>
    {
        void ReceiveEvent(T @event);
    }
}
