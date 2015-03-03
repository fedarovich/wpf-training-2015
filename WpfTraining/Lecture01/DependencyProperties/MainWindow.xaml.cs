using System.Windows;

namespace DependencyProperties
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", 
            typeof(int), 
            typeof(MainWindow), 
            new PropertyMetadata(0, OnMinimumChanged),
            ValidateRange);

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", 
            typeof(int), 
            typeof(MainWindow), 
            new PropertyMetadata(100, null, OnCoerceMaximum),
            ValidateRange);

        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        private static bool ValidateRange(object value)
        {
            int intValue = (int) value;
            return intValue >= 0 && intValue <= 100;
        }


        public MainWindow()
        {
            InitializeComponent();
        }

        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = (MainWindow)d;
            window.CoerceValue(MaximumProperty);
        }
        
        private static object OnCoerceMaximum(DependencyObject d, object basevalue)
        {
            var window = (MainWindow) d;
            var value = (int) basevalue;
            return value >= window.Minimum ? value : window.Minimum;
        }
    }
}
