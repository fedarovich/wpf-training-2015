using System.Windows;

namespace DemoControls.Design
{
    /// <summary>
    /// Interaction logic for TextEditorDialog.xaml
    /// </summary>
    public partial class TextEditorDialog : Window
    {
        public TextEditorDialog()
        {
            InitializeComponent();
        }
        
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextEditorDialog), new PropertyMetadata(""));

        private void OnAccept(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
