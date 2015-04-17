using System;
using System.Collections.Generic;
using System.Globalization;

namespace CustomMarkupExtensions
{
    public static class LocalizationManager
    {
        private static CultureInfo _currentUiCulture = CultureInfo.GetCultureInfo("en-US");

        public static IEnumerable<CultureInfo> Cultures
        {
            get
            {
                yield return CultureInfo.GetCultureInfo("en-US");
                yield return CultureInfo.GetCultureInfo("de-DE");
            }
        }

        public static CultureInfo CurrentUICulture
        {
            get { return _currentUiCulture; }
            set
            {
                _currentUiCulture = value;
                CurrentUICultureChanged(null, EventArgs.Empty);
            }
        }

        public static event EventHandler CurrentUICultureChanged = delegate {};
    }
}
