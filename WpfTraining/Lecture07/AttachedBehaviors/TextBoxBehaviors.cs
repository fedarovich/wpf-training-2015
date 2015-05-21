using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace AttachedBehaviors
{
    public static class TextBoxBehaviors
    {
        #region SelectAllWhenGotFocus

        public static readonly DependencyProperty SelectAllOnFocusProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnFocus", typeof (bool), typeof (TextBoxBehaviors),
            new PropertyMetadata(false, OnSelectAllWhenGotFocusChanged));

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetSelectAllOnFocus(TextBox textBox)
        {
            return (bool)textBox.GetValue(SelectAllOnFocusProperty);
        }

        public static void SetSelectAllOnFocus(TextBox textBox, bool value)
        {
            textBox.SetValue(SelectAllOnFocusProperty, value);
        }

        private static void OnSelectAllWhenGotFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox == null)
                return;

            if (true.Equals(e.NewValue))
            {
                textBox.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDownForSelectAllOnFocus;
                textBox.GotKeyboardFocus += OnGotFocusForSelectAllOnFocus;
            }
            else
            {
                textBox.GotKeyboardFocus -= OnGotFocusForSelectAllOnFocus;
                textBox.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDownForSelectAllOnFocus;
            }
        }

        private static void OnGotFocusForSelectAllOnFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = ((TextBox)sender);
            textBox.SelectAll();
        }

        private static void OnPreviewMouseLeftButtonDownForSelectAllOnFocus(object sender, MouseButtonEventArgs e)
        {
            // Find the TextBox
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
                parent = VisualTreeHelper.GetParent(parent);

            if (parent != null)
            {
                var textBox = (TextBox)parent;
                if (!textBox.IsKeyboardFocusWithin)
                {
                    // If the text box is not yet focussed, give it the focus and
                    // stop further processing of this click event.
                    textBox.Focus();
                    e.Handled = true;
                }
            }
        }

        #endregion

        #region IntegerOnly

        public static readonly DependencyProperty IntegerOnlyProperty = DependencyProperty.RegisterAttached(
            "IntegerOnly", typeof (bool), typeof (TextBoxBehaviors),
            new PropertyMetadata(false, OnIntegerOnlyChanged));

        public static bool GetIntegerOnly(TextBox textBox)
        {
            return (bool)textBox.GetValue(SelectAllOnFocusProperty);
        }

        public static void SetIntegerOnly(TextBox textBox, bool value)
        {
            textBox.SetValue(SelectAllOnFocusProperty, value);
        }

        private static void OnIntegerOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox == null)
                return;

            if (true.Equals(e.NewValue))
            {
                textBox.PreviewTextInput += OnPreviewTextInputForIntegerOnly;
                textBox.PreviewKeyDown += OnPreviewKeyDownForIntegerOnly;
                DataObject.AddPastingHandler(textBox, OnPastingForIntegerOnly);
            }
            else
            {
                DataObject.RemovePastingHandler(textBox, OnPastingForIntegerOnly);
                textBox.PreviewKeyDown -= OnPreviewKeyDownForIntegerOnly;
                textBox.PreviewTextInput -= OnPreviewTextInputForIntegerOnly;
            }
        }

        private static void OnPreviewKeyDownForIntegerOnly(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private static void OnPreviewTextInputForIntegerOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValidInput((TextBox)sender, e.Text);
        }

        private static void OnPastingForIntegerOnly(object sender, DataObjectPastingEventArgs e)
        {
            if (!e.SourceDataObject.GetDataPresent(DataFormats.Text, true))
            {
                e.CancelCommand();
            }
            else
            {
                var text = (string)e.SourceDataObject.GetData(DataFormats.Text, true);
                if (!IsValidInput((TextBox)sender, text))
                {
                    e.CancelCommand();
                }
            }
        }

        private static bool IsValidInput(TextBox textBox, string inputText)
        {
            // Concatenate the text before selection, the input text and the text after selection to get the resulting text to validate.
            var prevText = textBox.Text;
            var prevStart = prevText.Substring(0, textBox.SelectionStart);
            var prevEnd = prevText.Substring(textBox.SelectionStart + textBox.SelectionLength);
            var newText = string.Concat(prevStart, inputText, prevEnd);
            // Allow text containing only "-" to simplify entering negative values.
            if (newText == "-")
            {
                return true;
            }

            long result;
            return long.TryParse(newText, out result);
        }

        #endregion
    }
}
