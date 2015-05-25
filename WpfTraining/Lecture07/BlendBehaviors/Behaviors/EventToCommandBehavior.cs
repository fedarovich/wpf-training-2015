using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace BlendBehaviors.Behaviors
{
    public class EventToCommandBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty EventNameProperty =
            DependencyProperty.Register("EventName", typeof(string), typeof(EventToCommandBehavior), 
                new PropertyMetadata(null, OnEventNameChanged));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof (ICommand), typeof (EventToCommandBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(EventToCommandBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty HandledEventsTooProperty =
            DependencyProperty.Register("HandledEventsToo", typeof(bool), typeof(EventToCommandBehavior),
            new PropertyMetadata(false, OnHandledEventsTooChanged));

        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }


        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }


        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }


        public bool HandledEventsToo
        {
            get { return (bool)GetValue(HandledEventsTooProperty); }
            set { SetValue(HandledEventsTooProperty, value); }
        }

        protected override void OnAttached()
        {
            var routedEvent = FindEvent(EventName);
            Subscribe(routedEvent);
        }

        protected override void OnDetaching()
        {
            var routedEvent = FindEvent(EventName);
            Unsubscribe(routedEvent);
        }

        private static void OnEventNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (EventToCommandBehavior) d;
            behavior.Unsubscribe(behavior.FindEvent((string)e.OldValue));
            behavior.Subscribe(behavior.FindEvent((string)e.NewValue));
        }


        private static void OnHandledEventsTooChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (EventToCommandBehavior)d;
            var routedEvent = behavior.FindEvent(behavior.EventName);
            behavior.Unsubscribe(routedEvent);
            behavior.Subscribe(routedEvent);
        }


        private RoutedEvent FindEvent(string eventName)
        {
            if (string.IsNullOrWhiteSpace(eventName) || AssociatedObject == null)
                return null;

            var routedEventField = AssociatedObject.GetType().GetField(
                eventName + "Event",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            if (routedEventField == null)
                return null;

            return routedEventField.GetValue(null) as RoutedEvent;
        }

        private void Subscribe(RoutedEvent routedEvent)
        {
            if (routedEvent != null && AssociatedObject != null)
            {
                AssociatedObject.AddHandler(routedEvent, new RoutedEventHandler(OnEvent), HandledEventsToo);
            }
        }

        private void Unsubscribe(RoutedEvent routedEvent)
        {
            if (routedEvent != null && AssociatedObject != null)
            {
                AssociatedObject.RemoveHandler(routedEvent, new RoutedEventHandler(OnEvent));
            }
        }

        private void OnEvent(object sender, RoutedEventArgs e)
        {
            if (Command != null)
            {
                if (Command.CanExecute(CommandParameter))
                {
                    Command.Execute(CommandParameter);
                }
            }
        }
    }
}
