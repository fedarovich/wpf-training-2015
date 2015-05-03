using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Trees
{
    /// <summary>
    /// Interaction logic for LogicalTreeViewer.xaml
    /// </summary>
    public partial class LogicalTreeViewer : Window
    {
        public static readonly DependencyProperty RootProperty = DependencyProperty.Register(
            "Root", typeof(DependencyObject), typeof(LogicalTreeViewer), 
            new PropertyMetadata(null, OnRootChanged));

        private static readonly DependencyPropertyKey ItemsPropertyKey = DependencyProperty.RegisterReadOnly(
            "Items", typeof(IEnumerable<LogicalNode>), typeof(LogicalTreeViewer), new PropertyMetadata(null));

        public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

        public DependencyObject Root
        {
            get { return (DependencyObject)GetValue(RootProperty); }
            set { SetValue(RootProperty, value); }
        }

        public IEnumerable<LogicalNode> Items
        {
            get { return (IEnumerable<LogicalNode>)GetValue(ItemsProperty); }
            private set { SetValue(ItemsPropertyKey, value); }
        }

        private static void OnRootChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tree = (LogicalTreeViewer)d;
            if (tree.Root != null)
            {
                tree.Items = new[] { new LogicalNode(tree.Root) };
            }
            else
            {
                tree.Items = Enumerable.Empty<LogicalNode>();
            }
        }

        public LogicalTreeViewer()
        {
            InitializeComponent();
        }
    }
}
