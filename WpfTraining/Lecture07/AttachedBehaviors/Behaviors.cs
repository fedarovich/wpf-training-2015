using System.Windows;

namespace AttachedBehaviors
{
    public static class Behaviors
    {
        public static readonly DependencyProperty AutoFocusProperty = DependencyProperty.RegisterAttached(
            "AutoFocus", typeof (bool), typeof (Behaviors),
            new PropertyMetadata(false, OnAutoFocusChanged));

        public static bool GetAutoFocus(FrameworkElement element)
        {
            return (bool)element.GetValue(AutoFocusProperty);
        }

        public static void SetAutoFocus(FrameworkElement element, bool value)
        {
            element.SetValue(AutoFocusProperty, value);
        }

        private static void OnAutoFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if (element == null)
                return;

            if (true.Equals(e.NewValue))
            {
                element.Loaded += OnLoadedForAutoFocus;
            }
            else
            {
                element.Loaded -= OnLoadedForAutoFocus;
            }
        }

        private static void OnLoadedForAutoFocus(object sender, RoutedEventArgs e)
        {
            ((FrameworkElement) sender).Focus();
        }
    }
}
