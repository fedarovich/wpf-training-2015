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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Trees
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VisualTreeViewer visualTreeViewer;
        private LogicalTreeViewer logicalTreeViewer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowVisualTree(object sender, RoutedEventArgs e)
        {
            if (visualTreeViewer != null)
            {
                visualTreeViewer.Activate();
            }
            else
            {
                visualTreeViewer = new VisualTreeViewer {Root = this};
                visualTreeViewer.Closed += delegate { visualTreeViewer = null; };
                visualTreeViewer.Show();
            }
        }

        private void ShowLogicalTree(object sender, RoutedEventArgs e)
        {
            if (logicalTreeViewer != null)
            {
                logicalTreeViewer.Activate();
            }
            else
            {
                logicalTreeViewer = new LogicalTreeViewer {Root = this};
                logicalTreeViewer.Closed += delegate { logicalTreeViewer = null; };
                logicalTreeViewer.Show();
            }
        }
    }
}
