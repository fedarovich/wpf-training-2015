using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WeakEvents.Handlers;

namespace WeakEvents
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<StrongHandler> strongHandlers = 
            new ObservableCollection<StrongHandler>();

        private readonly ObservableCollection<WeakHandler> weakHandlers =
            new ObservableCollection<WeakHandler>();

        private readonly ObservableCollection<GenericWeakHandler> genericWeakHandlers =
            new ObservableCollection<GenericWeakHandler>(); 

        public MainWindow()
        {
            InitializeComponent();
        }

        public IList<StrongHandler> StrongHandlers
        {
            get { return strongHandlers; }
        }

        public IList<WeakHandler> WeakHandlers
        {
            get { return weakHandlers; }
        }

        public IList<GenericWeakHandler> GenericWeakHandlers
        {
            get { return genericWeakHandlers; }
        }

        private void OnCollect(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void OnAddStrongHandler(object sender, RoutedEventArgs e)
        {
            var handler = new StrongHandler();
            NotifyButton.Click += handler.OnClick;
            strongHandlers.Add(handler);
        }

        private void OnAddWeakHandler(object sender, RoutedEventArgs e)
        {
            var handler = new WeakHandler();
            ClickEventManager.AddListener(NotifyButton, handler);
            weakHandlers.Add(handler);
        }

        private void OnAddGenericWeakHandler(object sender, RoutedEventArgs e)
        {
            var handler = new GenericWeakHandler();
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(NotifyButton, "Click", handler.OnClick);
            genericWeakHandlers.Add(handler);
        }

        private void OnClearStrongHandlers(object sender, RoutedEventArgs e)
        {
            strongHandlers.Clear();
        }

        private void OnClearWeakHandlers(object sender, RoutedEventArgs e)
        {
            weakHandlers.Clear();
        }

        private void OnClearGenericWeakHandlers(object sender, RoutedEventArgs e)
        {
            genericWeakHandlers.Clear();
        }
    }
}
