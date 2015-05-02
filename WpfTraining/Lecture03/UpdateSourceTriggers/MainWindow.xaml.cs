using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace UpdateSourceTriggers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            var bindingExpression = 
                BindingOperations.GetBindingExpression(ExplicitTextBox, TextBox.TextProperty);
            bindingExpression.UpdateSource();
        }
    }
}
