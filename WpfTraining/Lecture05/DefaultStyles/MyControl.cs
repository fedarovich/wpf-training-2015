using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DefaultStyles
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:DefaultStyles"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:DefaultStyles;assembly=DefaultStyles"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:MyControl/>
    ///
    /// </summary>
    public class MyControl : Control
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(MyControl), new PropertyMetadata("Default Property Text"));

        static MyControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(MyControl), 
                new FrameworkPropertyMetadata(typeof(MyControl)));

            // Let's override default value for backgound
            BackgroundProperty.OverrideMetadata(
                typeof(MyControl),
                new FrameworkPropertyMetadata(Brushes.Aquamarine));
        }
    }
}
