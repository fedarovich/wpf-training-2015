using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using BlendBehaviors.Behaviors;

namespace BlendBehaviors.Views
{
    /// <summary>
    /// Interaction logic for DoubleInputView.xaml
    /// </summary>
    public partial class DoubleInputView : UserControl
    {
        public DoubleInputView()
        {
            InitializeComponent();
        }

        public IList<CultureInfo> AllCultures
        {
            get
            {
                return CultureInfo.GetCultures(CultureTypes.AllCultures)
                    .OrderBy(x => x.Name)
                    .ToList();
            }
        }

        public IList<DecimalKeyInputMode> DecimalKeyInputModes
        {
            get { return new[] {DecimalKeyInputMode.DecimalSeparator, DecimalKeyInputMode.Native}; }
        }
    }
}
