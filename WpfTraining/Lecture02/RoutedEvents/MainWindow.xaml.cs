using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace RoutedEvents
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            EventLog = new ObservableCollection<EventViewModel>();

            dockPanel.AddHandler(Button.ClickEvent, new RoutedEventHandler(OnDockPanelButtonClick));
            
            this.AddHandler(Button.ClickEvent, new RoutedEventHandler(OnWindowButtonClick), true);
        }

        public IList<EventViewModel> EventLog { get; private set; } 

        private void OnButton1Click(object sender, RoutedEventArgs e)
        {
            EventLog.Add(CreateViewModel(sender, e));
        }

        private void OnButton2Click(object sender, RoutedEventArgs e)
        {
            EventLog.Add(CreateViewModel(sender, e));
        }

        private void OnButton3Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            EventLog.Add(CreateViewModel(sender, e));
        }

        private void OnButton4Click(object sender, RoutedEventArgs e)
        {
            EventLog.Add(CreateViewModel(sender, e));
        }

        private void OnUniformGridButtonClick(object sender, RoutedEventArgs e)
        {
            var button = (Button) e.Source;
            if (button.Name == "button2")
            {
                e.Handled = true;
            }
            EventLog.Add(CreateViewModel(sender, e));
        }

        private void OnDockPanelButtonClick(object sender, RoutedEventArgs e)
        {
            EventLog.Add(CreateViewModel(sender, e));
        }

        private void OnWindowButtonClick(object sender, RoutedEventArgs e)
        {
            EventLog.Add(CreateViewModel(sender, e));
        }

        private EventViewModel CreateViewModel(object sender, RoutedEventArgs e)
        {
            return new EventViewModel(sender, e);
        }

        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Dispatcher.BeginInvoke(new Action(EventLog.Clear));
        }
    }
}
