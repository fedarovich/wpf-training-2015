using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RoutedEvents
{
    public class EventViewModel
    {
        public EventViewModel(object sender, RoutedEventArgs e)
        {
            Sender = sender != null ? sender.ToString() : null;
            Source = e.Source != null ? e.Source.ToString() : null;
            OriginalSource = e.OriginalSource != null ? e.OriginalSource.ToString() : null;
            Handled = e.Handled;
        }

        public string Sender { get; private set; }

        public string Source { get; private set; }

        public string OriginalSource { get; private set; }

        public bool Handled { get; private set; }
    }
}
