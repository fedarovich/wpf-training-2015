using System;
using System.Collections.Generic;
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

namespace StaticVsDynamicResources
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random random;
        private SolidColorBrush originalBrush;

        public MainWindow()
        {
            InitializeComponent();
            random = new Random();
            originalBrush = (SolidColorBrush) Resources["MyBrush"];
        }

        private void ChangeOriginalBrushColor(object sender, RoutedEventArgs e)
        {
            originalBrush.Color = GetRandomColor();
        }

        private void ChangeCurrentBrushColor(object sender, RoutedEventArgs e)
        {
            var brush = (SolidColorBrush) Resources["MyBrush"];
            brush.Color = GetRandomColor();
        }

        private void ChangeResource(object sender, RoutedEventArgs e)
        {
            Resources["MyBrush"] = new SolidColorBrush(GetRandomColor());
        }

        private Color GetRandomColor()
        {
            var channels = new byte[3];
            random.NextBytes(channels);
            return Color.FromRgb(channels[0], channels[1], channels[2]);
        }

        private void AddBrushToGrid(object sender, RoutedEventArgs e)
        {
            grid.Resources["MyBrush"] = new SolidColorBrush(GetRandomColor());
        }

        private void RemoveBrushFromGrid(object sender, RoutedEventArgs e)
        {
            grid.Resources.Remove("MyBrush");
        }
    }
}
