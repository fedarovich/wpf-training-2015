using System.Windows;
using System.Windows.Controls;

namespace ChangeNotifications.Views
{
    /// <summary>
    /// Interaction logic for DependencyObjectSample.xaml
    /// </summary>
    public partial class DependencyObjectSample : UserControl
    {
        public int ClickCount
        {
            get { return (int)GetValue(ClickCountProperty); }
            set { SetValue(ClickCountProperty, value); }
        }

        public static readonly DependencyProperty ClickCountProperty =
            DependencyProperty.Register("ClickCount", typeof(int), typeof(DependencyObjectSample), new PropertyMetadata(0));
        
        public DependencyObjectSample()
        {
            InitializeComponent();
        }

        private void IncreaseValue(object sender, RoutedEventArgs e)
        {
            ClickCount += 1;
        }
    }
}
