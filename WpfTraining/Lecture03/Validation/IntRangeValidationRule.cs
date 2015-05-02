using System;
using System.Globalization;
using System.Windows.Controls;

namespace Validation
{
    public class IntRangeValidationRule : ValidationRule
    {
        public IntRangeValidationRule() : base(ValidationStep.ConvertedProposedValue, true)
        {
            Maximum = 100;
        }

        public int Minimum { get; set; }

        public int Maximum { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int intValue = (int) value;
            if (intValue >= Minimum && intValue <= Maximum)
                return ValidationResult.ValidResult;

            return new ValidationResult(false, 
                string.Format("The value must be in range [{0}, {1}]", Minimum, Maximum));
        }
    }
}
