using System.Windows;
using System.Windows.Controls;

namespace RoutedEvents
{
    public class MyButton : Button
    {
        public static readonly RoutedEvent MyClickEvent = EventManager.RegisterRoutedEvent(
            "MyClick",
            RoutingStrategy.Bubble, 
            typeof (RoutedEventHandler), 
            typeof (MyButton));

        public event RoutedEventHandler MyClick
        {
            add { AddHandler(MyClickEvent, value); }
            remove { RemoveHandler(MyClickEvent, value); }
        }

        protected override void OnClick()
        {
            RaiseEvent(new RoutedEventArgs(MyClickEvent));
        }
    }
}
