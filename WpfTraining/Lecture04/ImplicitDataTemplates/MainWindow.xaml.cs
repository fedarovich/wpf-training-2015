using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImplicitDataTemplates
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IList<object> Items
        {
            get { return (IList<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            "Items", typeof(IList<object>), typeof(MainWindow), new PropertyMetadata(null));
        
        public MainWindow()
        {
            InitializeComponent();
            Items = new ObservableCollection<object>();
        }

        private void AddLeft(object sender, RoutedEventArgs e)
        {
            Items.Add(new LeftItem());
        }

        private void AddCentered(object sender, RoutedEventArgs e)
        {
            Items.Add(new CenteredItem());
        }

        private void AddRight(object sender, RoutedEventArgs e)
        {
            Items.Add(new RightItem());
        }
    }
}
