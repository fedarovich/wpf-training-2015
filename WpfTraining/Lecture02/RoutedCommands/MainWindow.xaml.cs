using System.Windows;
using System.Windows.Input;

namespace RoutedCommands
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool IsOpened
        {
            get { return (bool)GetValue(IsOpenedProperty); }
            set { SetValue(IsOpenedProperty, value); }
        }

        public static readonly DependencyProperty IsOpenedProperty =
            DependencyProperty.Register("IsOpened", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        
        public MainWindow()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Print, OnPrintCommandExecuted));
            InputBindings.Add(new KeyBinding(ApplicationCommands.Print, Key.P, ModifierKeys.Control));
        }

        static MainWindow()
        {
            CommandManager.RegisterClassCommandBinding(
                typeof(MainWindow), new CommandBinding(ApplicationCommands.Close, OnCloseCommandExecuted));
        }

        private void OnOpenCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show(Format(sender, e));
            IsOpened = true;
        }

        private void OnSaveCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show(Format(sender, e));
        }

        private void OnSaveCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsOpened;
        }

        private void OnPrintCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show(Format(sender, e));
        }
        
        private static void OnCloseCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var window = sender as MainWindow;
            if (MessageBox.Show("Close?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                window.IsOpened = false;
            }
        }

        private string Format(object sender, ExecutedRoutedEventArgs e)
        {
            var routedUICommand = (RoutedUICommand)e.Command;
            return string.Format("{0} command executed.\r\nSender: {1}\r\nSource: {2}",
                routedUICommand.Text,
                sender,
                e.OriginalSource);
        }

    }
}
