using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace DemoMvvm
{
    public class EventToCommandAction : TriggerAction<FrameworkElement>
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(EventToCommandAction), 
            new PropertyMetadata(null));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter", typeof(object), typeof(EventToCommandAction),
            new PropertyMetadata(null, null, OnCoerceCommandParameter));

        public static readonly DependencyProperty UseActionParameterProperty = DependencyProperty.Register(
            "UseActionParameter", typeof(bool), typeof(EventToCommandAction), 
            new PropertyMetadata(false, OnUseActionParameterChanged));


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

        public bool UseActionParameter
        {
            get { return (bool)GetValue(UseActionParameterProperty); }
            set { SetValue(UseActionParameterProperty, value); }
        }

        protected override void Invoke(object parameter)
        {
            object commandParameter = UseActionParameter ? parameter : CommandParameter;
            if (Command != null && Command.CanExecute(commandParameter))
            {
                Command.Execute(commandParameter);
            }
        }

        private static object OnCoerceCommandParameter(DependencyObject d, object basevalue)
        {
            var action = (EventToCommandAction)d;
            if (action.UseActionParameter && basevalue != null)
                return DependencyProperty.UnsetValue;
            return basevalue;
        }

        private static void OnUseActionParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var action = (EventToCommandAction)d;
            action.CoerceValue(CommandParameterProperty);
        }
    }
}
