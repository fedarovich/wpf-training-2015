using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMvvm
{
    public interface IEventAggregator
    {
        void Subscribe(object subscriber);

        void Unsubscribe(object subscriber);

        void Publish(object message, Action<Action> marshal = null);
    }
}
