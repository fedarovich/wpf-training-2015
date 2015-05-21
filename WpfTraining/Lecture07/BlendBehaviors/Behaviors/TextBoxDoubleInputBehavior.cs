using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace BlendBehaviors.Behaviors
{
    /// <summary>
    /// Provides behavior to filter the <see cref="TextBox" /> keyboard input. This allows you to limit the input to
    /// correct <see cref="double" /> values.
    /// </summary>
    public class TextBoxDoubleInputBehavior : Behavior<TextBox>
    {
        #region Constants

        /// <summary>
        /// The dependency property for the <see cref="DecimalPlaces" /> property.
        /// </summary>
        public static readonly DependencyProperty DecimalPlacesProperty =
            DependencyProperty.Register("DecimalPlaces", typeof(int), typeof(TextBoxDoubleInputBehavior),
                new PropertyMetadata(15), ValidateDecimalPlaces);

        /// <summary>
        /// The dependency property for the <see cref="AllowNegative" /> property.
        /// </summary>
        public static readonly DependencyProperty AllowNegativeProperty =
            DependencyProperty.Register("AllowNegative", typeof(bool), typeof(TextBoxDoubleInputBehavior),
                new PropertyMetadata(true));

        /// <summary>
        /// The dependency property for the <see cref="Culture" /> property.
        /// </summary>
        public static readonly DependencyProperty CultureProperty =
            DependencyProperty.Register("Culture", typeof(CultureInfo), typeof(TextBoxDoubleInputBehavior),
                new PropertyMetadata(null));

        /// <summary>
        /// The dependency property for the <see cref="DecimalKeyInputMode" /> property.
        /// </summary>
        public static readonly DependencyProperty DecimalKeyInputModeProperty =
            DependencyProperty.Register("DecimalKeyInputMode", typeof(DecimalKeyInputMode), typeof(TextBoxDoubleInputBehavior),
                new PropertyMetadata(DecimalKeyInputMode.DecimalSeparator));

        /// <summary>
        /// The dependency property for the <see cref="AlwaysAcceptCommaAsDecimalSeparator" /> property.
        /// </summary>
        public static readonly DependencyProperty AlwaysAcceptCommaAsDecimalSeparatorProperty =
            DependencyProperty.Register("AlwaysAcceptCommaAsDecimalSeparator",
                typeof(bool), typeof(TextBoxDoubleInputBehavior), new PropertyMetadata(true));

        /// <summary>
        /// The dependency property for the <see cref="AlwaysAcceptPeriodAsDecimalSeparator" /> property.
        /// </summary>
        public static readonly DependencyProperty AlwaysAcceptPeriodAsDecimalSeparatorProperty =
            DependencyProperty.Register("AlwaysAcceptPeriodAsDecimalSeparator", typeof(bool), typeof(TextBoxDoubleInputBehavior),
                new PropertyMetadata(true));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the number of decimal places.
        /// </summary>
        public int DecimalPlaces
        {
            get { return (int)GetValue(DecimalPlacesProperty); }
            set { SetValue(DecimalPlacesProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether negative values are accepted.
        /// </summary>
        public bool AllowNegative
        {
            get { return (bool)GetValue(AllowNegativeProperty); }
            set { SetValue(AllowNegativeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the culture for the number format. If not set, <see cref="CultureInfo.CurrentCulture" /> will be used.
        /// </summary>
        public CultureInfo Culture
        {
            get { return (CultureInfo)GetValue(CultureProperty); }
            set { SetValue(CultureProperty, value); }
        }

        /// <summary>
        /// <para>
        /// Gets or sets the input mode for the decimal key (located on the keypad of the keyboard).
        /// </para>
        /// <para>
        /// If the mode is set to <see cref="Behaviors.DecimalKeyInputMode.Native" />, the decimal
        /// key will
        /// be handled by the OS.
        /// </para>
        /// <para>
        /// If the mode is set to <see cref="Behaviors.DecimalKeyInputMode.DecimalSeparator" />
        /// the decimal separator of the current <see cref="Culture" /> will be inserted.
        /// </para>
        /// <para>
        /// The default value is <see cref="Behaviors.DecimalKeyInputMode.DecimalSeparator" />.
        /// </para>
        /// </summary>
        public DecimalKeyInputMode DecimalKeyInputMode
        {
            get { return (DecimalKeyInputMode)GetValue(DecimalKeyInputModeProperty); }
            set { SetValue(DecimalKeyInputModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control will always accept the comma symbol as decimal separator
        /// regardless of the culture settings.
        /// </summary>
        /// <value>
        /// <see langword="True" /> if the control will always accept the comma symbol as a decimal separator.
        /// <see langword="False" /> if the control will only accept the comma key as a decimal separator if it is set
        /// as the decimal separator in the current <see cref="Culture" />.
        /// </value>
        /// <remarks>
        /// If the comma symbol is not the current decimal separator, the decimal separator defined by the specified
        /// <see cref="Culture" /> will be inserted when the user presses the comma key.
        /// </remarks>
        public bool AlwaysAcceptCommaAsDecimalSeparator
        {
            get { return (bool)GetValue(AlwaysAcceptCommaAsDecimalSeparatorProperty); }
            set { SetValue(AlwaysAcceptCommaAsDecimalSeparatorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control will always accept the period symbol as decimal separator
        /// regardless of the culture settings.
        /// </summary>
        /// <value>
        /// <see langword="True" /> if the control will always accept the period symbol as a decimal separator.
        /// <see langword="False" /> if the control will only accept the period key as a decimal separator if it is set
        /// as the decimal separator in the current <see cref="Culture" />.
        /// </value>
        /// <remarks>
        /// If the period symbol is not the current decimal separator, the decimal separator defined by the specified
        /// <see cref="Culture" /> will be inserted when the user presses the period key.
        /// </remarks>
        public bool AlwaysAcceptPeriodAsDecimalSeparator
        {
            get { return (bool)GetValue(AlwaysAcceptPeriodAsDecimalSeparatorProperty); }
            set { SetValue(AlwaysAcceptPeriodAsDecimalSeparatorProperty, value); }
        }

        /// <summary>
        /// Gets the specified culture.
        /// </summary>
        /// <value>The specified culture.</value>
        /// <remarks>
        /// If <see cref="Culture" /> is not <see langword="null" />, its value is returned.
        /// Otherwise, <see cref="CultureInfo.CurrentCulture" /> is returned.
        /// </remarks>
        protected CultureInfo ActualCulture
        {
            get { return Culture ?? CultureInfo.CurrentCulture; }
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

        private static bool ValidateDecimalPlaces(object value)
        {
            var intValue = (int)value;
            return intValue >= 0 && intValue <= 15;
        }

        /// <summary>
        /// Sends the decimal separator to the focused element.
        /// </summary>
        /// <param name="separator">The separator.</param>
        private static void SendDecimalSeparator(string separator)
        {
            // Send the specified separator text to the focused element.
            // Send it using TextCompositionManager.PreviewTextInputEvent first and if the event
            // is not marked as Handled, raise TextCompositionManager.TextInputEvent.
            var composition = new TextComposition(InputManager.Current, Keyboard.FocusedElement, separator);
            var args = new TextCompositionEventArgs(InputManager.Current.PrimaryKeyboardDevice, composition)
            {
                RoutedEvent = TextCompositionManager.PreviewTextInputEvent
            };
            Keyboard.FocusedElement.RaiseEvent(args);
            if (!args.Handled)
            {
                args.RoutedEvent = TextCompositionManager.TextInputEvent;
                Keyboard.FocusedElement.RaiseEvent(args);
            }
        }

        /// <summary>
        /// Gets the string corresponding to the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The string corresponding to the key.</returns>
        private static string GetTextFromKey(Key key)
        {
            // Get WinAPI virtual key from WPF key.
            var virtualKey = (uint)KeyInterop.VirtualKeyFromKey(key);
            // Get the WinAPI keyboard state.
            var keyboardState = new byte[256];
            NativeMethods.GetKeyboardState(keyboardState);
            // Convert the virtual key to the scan code.
            uint scanCode = NativeMethods.MapVirtualKey(virtualKey, NativeMethods.MapType.MAPVK_VK_TO_VSC);
            var buffer = new StringBuilder(8);
            // Convert the keyboard state and the scan code to the corresponding Unicode text.
            int length = NativeMethods.ToUnicode(virtualKey, scanCode, keyboardState, buffer, buffer.Capacity, 0);
            return length > 0 ? buffer.ToString(0, length) : string.Empty;
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
                var text = (string)e.SourceDataObject.GetData(DataFormats.Text, true);
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

            double result;
            if (!double.TryParse(newText, GetNumberStyles(), ActualCulture, out result))
            {
                return false;
            }

            int decimalPointIndex = ActualCulture.CompareInfo.LastIndexOf(
                newText, ActualCulture.NumberFormat.NumberDecimalSeparator);
            if (decimalPointIndex < 0)
            {
                return true;
            }
            // Check whether the decimal separator is located within the allowed decimal places range.
            var places = newText.Length - decimalPointIndex - 1;
            return places <= DecimalPlaces;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }

            // Check whether the key (".", "," or numpad ".") must be replaced with the valid decimal separator for the current culture.
            if (MustReplaceDecimalKey(e))
            {
                var separator = ActualCulture.NumberFormat.NumberDecimalSeparator;
                if (separator.Length > 0)
                {
                    // If yes, do no input this key but send the valid decimal separator key.
                    e.Handled = true;
                    SendDecimalSeparator(separator);
                }
            }
        }

        private bool MustReplaceDecimalKey(KeyEventArgs e)
        {
            if (e.Key == Key.Decimal && DecimalKeyInputMode == DecimalKeyInputMode.DecimalSeparator)
                return true;

            var separator = ActualCulture.NumberFormat.NumberDecimalSeparator;
            if (AlwaysAcceptCommaAsDecimalSeparator && separator != "," && GetTextFromKey(e.Key) == ",")
                return true;
            if (AlwaysAcceptPeriodAsDecimalSeparator && separator != "." && GetTextFromKey(e.Key) == ".")
                return true;

            return false;
        }

        private NumberStyles GetNumberStyles()
        {
            var styles = NumberStyles.None;
            if (DecimalPlaces > 0)
            {
                styles |= NumberStyles.AllowDecimalPoint;
            }
            if (AllowNegative)
            {
                styles |= NumberStyles.AllowLeadingSign;
            }
            return styles;
        }

        #endregion
    }
}
