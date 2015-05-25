using System.Windows;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;

namespace BlendBehaviors.ViewModels
{
    public class EventToCommandViewModel
    {
        public EventToCommandViewModel()
        {
            ButtonClickCommand = new ActionCommand(() => MessageBox.Show("Button click."));
            ButtonMouseDownCommand = new ActionCommand(() => MessageBox.Show("Button mouse down."));
            BorderMouseDownCommand = new ActionCommand(() => MessageBox.Show("Border mouse down."));
        }

        public ICommand ButtonClickCommand { get; private set; }
        public ICommand ButtonMouseDownCommand { get; private set; }
        public ICommand BorderMouseDownCommand { get; private set; }
    }
}
