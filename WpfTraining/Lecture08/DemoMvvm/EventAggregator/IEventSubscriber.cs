using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMvvm
{
    public interface IEventSubscriber { }

    public interface IEventSubscriber<T> : IEventSubscriber
    {
        void ReceiveEvent(T @event);
    }
}
