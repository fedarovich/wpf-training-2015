using System.Globalization;

namespace BlendBehaviors.Behaviors
{
    /// <summary>
    /// Provides decimal key input modes for the <see cref="TextBoxDoubleInputBehavior"/>.
    /// </summary>
    public enum DecimalKeyInputMode
    {
        /// <summary>
        /// The decimal key is handled by the OS.
        /// </summary>
        Native,

        /// <summary>
        /// The decimal separator of the current <see cref="CultureInfo" /> will be inserted.
        /// </summary>
        DecimalSeparator
    }
}
