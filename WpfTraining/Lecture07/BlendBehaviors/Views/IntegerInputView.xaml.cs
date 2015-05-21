using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace BlendBehaviors.Views
{
    /// <summary>
    /// Interaction logic for IntegerInputView.xaml
    /// </summary>
    public partial class IntegerInputView : UserControl
    {
        public IntegerInputView()
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
    }
}
