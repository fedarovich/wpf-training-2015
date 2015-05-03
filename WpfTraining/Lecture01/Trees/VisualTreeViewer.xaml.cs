using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Trees
{
    /// <summary>
    /// Interaction logic for VisualTreeViewer.xaml
    /// </summary>
    public partial class VisualTreeViewer : Window
    {
        public static readonly DependencyProperty RootProperty = DependencyProperty.Register(
            "Root", typeof(DependencyObject), typeof(VisualTreeViewer), 
            new PropertyMetadata(null, OnRootChanged));

        private static readonly DependencyPropertyKey ItemsPropertyKey = DependencyProperty.RegisterReadOnly(
            "Items", typeof (IEnumerable<VisualNode>), typeof (VisualTreeViewer), new PropertyMetadata(null));

        public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

        public DependencyObject Root
        {
            get { return (DependencyObject)GetValue(RootProperty); }
            set { SetValue(RootProperty, value); }
        }

        public IEnumerable<VisualNode> Items
        {
            get { return (IEnumerable<VisualNode>) GetValue(ItemsProperty); }
            private set { SetValue(ItemsPropertyKey, value); }
        }

        private static void OnRootChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tree = (VisualTreeViewer) d;
            if (tree.Root != null)
            {
                tree.Items = new[] {new VisualNode(tree.Root)};
            }
            else
            {
                tree.Items = Enumerable.Empty<VisualNode>();
            }
        }
        
        public VisualTreeViewer()
        {
            InitializeComponent();
        }
    }
}
