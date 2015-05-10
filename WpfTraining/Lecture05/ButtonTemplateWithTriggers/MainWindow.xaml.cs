using System.Windows;

namespace ButtonTemplateWithTriggers
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

        private void OnMyButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked");
        }
    }
}
