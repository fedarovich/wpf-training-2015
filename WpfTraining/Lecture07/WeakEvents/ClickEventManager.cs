using System;
using System.Windows;
using System.Windows.Controls;

namespace WeakEvents
{
    public class ClickEventManager : WeakEventManager
    {
        private ClickEventManager()
        {
        }

        /// <summary>
        /// Add a listener to the given source's event.
        /// </summary>
        public static void AddListener(Button source, IWeakEventListener listener)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (listener == null)
                throw new ArgumentNullException("listener");

            CurrentManager.ProtectedAddListener(source, listener);
        }

        /// <summary>
        /// Remove a listener to the given source's event.
        /// </summary>
        public static void RemoveListener(Button source, IWeakEventListener listener)
        {
            if (listener == null)
                throw new ArgumentNullException("listener");

            CurrentManager.ProtectedAddListener(source, listener);
        }

        protected override void StartListening(object source)
        {
            ((Button) source).Click += OnClick;
        }

        protected override void StopListening(object source)
        {
            ((Button)source).Click -= OnClick;
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            DeliverEvent(sender, e);
        }

        private static ClickEventManager CurrentManager
        {
            get
            {
                Type managerType = typeof(ClickEventManager);
                var manager = (ClickEventManager)GetCurrentManager(managerType);

                // at first use, create and register a new manager
                if (manager == null)
                {
                    manager = new ClickEventManager();
                    SetCurrentManager(managerType, manager);
                }

                return manager;
            }
        }
    }
}
