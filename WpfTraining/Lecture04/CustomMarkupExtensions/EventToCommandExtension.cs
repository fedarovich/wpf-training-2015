using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using Expression = System.Linq.Expressions.Expression;

namespace CustomMarkupExtensions
{
    [MarkupExtensionReturnType(typeof(Delegate))]
    public class EventToCommandExtension : MarkupExtension
    {
        private readonly string commandName;

        public EventToCommandExtension(string commandName)
        {
            this.commandName = commandName;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var targetProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (targetProvider == null)
                throw new InvalidOperationException("Cannot get IProvideValueTarget.");

            var targetObject = targetProvider.TargetObject as FrameworkElement;
            if (targetObject == null)
                throw new InvalidOperationException("Target object must inherit FrameworkElement.");

            var memberInfo = targetProvider.TargetProperty as MemberInfo;
            if (memberInfo == null)
                throw new InvalidOperationException();

            var handlerType = GetEventHandlerType(memberInfo);

            var proxy = new Proxy();
            var binding = new Binding("DataContext." + commandName) {Source = targetObject};
            BindingOperations.SetBinding(proxy, Proxy.CommandProperty, binding);

            var method = typeof(Proxy).GetMethod("Execute");
            var lambda = Expression.Lambda(
                handlerType,
                Expression.Call(Expression.Constant(proxy), method),
                GetParameters(handlerType));

            return lambda.Compile();
        }

        private Type GetEventHandlerType(MemberInfo memberInfo)
        {
            Type eventHandlerType = null;
            if (memberInfo is EventInfo)
            {
                var info = memberInfo as EventInfo;
                var eventInfo = info;
                eventHandlerType = eventInfo.EventHandlerType;
            }
            else if (memberInfo is MethodInfo)
            {
                var info = memberInfo as MethodInfo;
                var methodInfo = info;
                ParameterInfo[] pars = methodInfo.GetParameters();
                eventHandlerType = pars[1].ParameterType;
            }

            return eventHandlerType;
        }

        private static IEnumerable<ParameterExpression> GetParameters(Type delegateType)
        {
            return delegateType
                .GetMethod("Invoke")
                .GetParameters()
                .Select(p => Expression.Parameter(p.ParameterType));
        }

        private class Proxy : DependencyObject
        {
            public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
                "Command", typeof(ICommand), typeof(Proxy), new PropertyMetadata(null));

            public ICommand Command
            {
                get { return (ICommand)GetValue(CommandProperty); }
                set { SetValue(CommandProperty, value); }
            }

            public void Execute()
            {
                if (Command != null && Command.CanExecute(null))
                {
                    Command.Execute(null);
                }
            }
        }
    }
}
