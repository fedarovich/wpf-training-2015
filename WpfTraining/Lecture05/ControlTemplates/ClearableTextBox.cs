using System.Windows;
using System.Windows.Controls;

namespace ControlTemplates
{
    [TemplatePart(Name = ClearButtonName, Type = typeof(Button))]
    [StyleTypedProperty(Property = nameof(ButtonStyle), StyleTargetType = typeof(Button))]
    public class ClearableTextBox : TextBox
    {
        private const string ClearButtonName = "PART_ClearButton";

        private Button _clearButton;

        #region ButtonStyle

        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register(
            "ButtonStyle", typeof(Style), typeof(ClearableTextBox), new PropertyMetadata(null));

        public Style ButtonStyle
        {
            get { return (Style) GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        #endregion

        static ClearableTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ClearableTextBox), 
                new FrameworkPropertyMetadata(typeof(ClearableTextBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_clearButton != null)
            {
                _clearButton.Click -= OnClearButtonClick;
            }

            _clearButton = GetTemplateChild(ClearButtonName) as Button;

            if (_clearButton != null)
            {
                _clearButton.Click += OnClearButtonClick;
            }
        }

        private void OnClearButtonClick(object sender, RoutedEventArgs e)
        {
            this.Clear();
        }
    }
}
