using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DemoControls
{
    public class TextView : Control
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(TextView), new PropertyMetadata(""));
        
        static TextView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(TextView),
                new FrameworkPropertyMetadata(typeof(TextView)));
        }
    }
}
