using Microsoft.Practices.Prism.PubSubEvents;
using PrismDemo.ViewModels;

namespace PrismDemo.Events
{
    public class DocumentClosedEvent : PubSubEvent<DocumentViewModel>
    {
    }
}
