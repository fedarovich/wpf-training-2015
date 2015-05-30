using System.ComponentModel;
using System.Windows;
using Microsoft.Practices.Prism.Mvvm;

namespace PrismDemo.Views
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window, IView
    {
        public Shell()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (Commands.CloseCommand.CanExecute(e))
            {
                Commands.CloseCommand.Execute(e);
            }
        }
    }
}
