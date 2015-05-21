using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace BlendBehaviors.Behaviors
{
    /// <summary>
    /// Filters the keyboard input to <see cref="TextBox"/> in order to allow users to enter only correct 
    /// <see cref="long"/> values.
    /// </summary>
    public class TextBoxIntegerInputBehavior : Behavior<TextBox>
    {
        #region Constants

        /// <summary>
        /// The dependency property for the <see cref="AllowNegative" /> property.
        /// </summary>
        public static readonly DependencyProperty AllowNegativeProperty =
            DependencyProperty.Register("AllowNegative", typeof (bool), typeof (TextBoxIntegerInputBehavior),
                new PropertyMetadata(true));

        /// <summary>
        /// The dependency property for the <see cref="Culture" /> property.
        /// </summary>
        public static readonly DependencyProperty CultureProperty =
            DependencyProperty.Register("Culture", typeof (CultureInfo), typeof (TextBoxIntegerInputBehavior),
                new PropertyMetadata(null));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether negative values are accepted.
        /// </summary>
        public bool AllowNegative
        {
            get { return (bool) GetValue(AllowNegativeProperty); }
            set { SetValue(AllowNegativeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the number culture. If not set, <see cref="CultureInfo.CurrentCulture" /> will be used.
        /// </summary>
        public CultureInfo Culture
        {
            get { return (CultureInfo) GetValue(CultureProperty); }
            set { SetValue(CultureProperty, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
        protected override void OnAttached()
        {
            AssociatedObject.PreviewTextInput += OnPreviewTextInput;
            AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
            DataObject.AddPastingHandler(AssociatedObject, OnPasting);
        }

        /// <summary>
        /// Invoked when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
        protected override void OnDetaching()
        {
            AssociatedObject.PreviewTextInput -= OnPreviewTextInput;
            AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;
            DataObject.RemovePastingHandler(AssociatedObject, OnPasting);
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValidInput(e.Text);
        }

        private void OnPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (!e.SourceDataObject.GetDataPresent(DataFormats.Text, true))
            {
                e.CancelCommand();
            }
            else
            {
                var text = (string) e.SourceDataObject.GetData(DataFormats.Text, true);
                if (!IsValidInput(text))
                {
                    e.CancelCommand();
                }
            }
        }

        private bool IsValidInput(string inputText)
        {
            // Concatenate the text before selection, the input text and the text after selection to get the resulting text to validate.
            var prevText = AssociatedObject.Text;
            var prevStart = prevText.Substring(0, AssociatedObject.SelectionStart);
            var prevEnd = prevText.Substring(AssociatedObject.SelectionStart + AssociatedObject.SelectionLength);
            var newText = string.Concat(prevStart, inputText, prevEnd);
            // Allow text containing only "-" to simplify entering negative values.
            if (newText == "-" && AllowNegative)
            {
                return true;
            }
            var culture = Culture ?? CultureInfo.CurrentCulture;
            long result;
            return long.TryParse(newText, GetNumberStyles(), culture, out result);
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private NumberStyles GetNumberStyles()
        {
            var styles = NumberStyles.None;
            if (AllowNegative)
            {
                styles |= NumberStyles.AllowLeadingSign;
            }
            return styles;
        }

        #endregion
    }
}
