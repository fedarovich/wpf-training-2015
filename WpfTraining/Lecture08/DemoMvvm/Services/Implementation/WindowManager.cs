using System.Windows;
using Microsoft.Practices.ServiceLocation;

namespace DemoMvvm.Services.Implementation
{
    public class WindowManager : IWindowManager
    {
        private readonly IViewResolver viewResolver;

        public WindowManager(IViewResolver viewResolver)
        {
            this.viewResolver = viewResolver;
        }

        public bool? ShowDialog(object viewModel)
        {
            var window = GetWindow(viewModel);
            if (window == null)
                return null;

            return window.ShowDialog();
        }

        public void ShowWindow(object viewModel)
        {
            var window = GetWindow(viewModel);
            if (window == null)
                return;

            window.Show();
        }

        private Window GetWindow(object viewModel)
        {
            if (viewModel == null)
                return null;

            var viewType = viewResolver.Resolve(viewModel.GetType());
            var view = ServiceLocator.Current.GetInstance(viewType) as FrameworkElement;
            if (view == null)
                return null;

            var window = view as Window ?? CreateWindow(view);
            window.DataContext = viewModel;
            return window;
        }

        private static Window CreateWindow(FrameworkElement view)
        {
            return new Window
            {
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
        }
    }
}
