using System;
using System.Windows;
using Microsoft.Practices.ServiceLocation;

namespace DemoMvvm
{
    public abstract class BootstrapperBase<TShellViewModel>
    {
        protected BootstrapperBase(Application application)
        {
            if (application == null)
                throw new ArgumentNullException();

            application.Startup += OnApplicationStartup;
        }

        protected virtual void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            ConfigureIoC();
            StartApplication();
        }

        protected abstract void ConfigureIoC();

        protected virtual void StartApplication()
        {
            var serviceLocator = ServiceLocator.Current;
            var shellViewModel = serviceLocator.GetInstance<TShellViewModel>();
            
            var viewResolver = serviceLocator.GetInstance<IViewResolver>();
            var shellViewType = viewResolver.Resolve(typeof (TShellViewModel));
            
            var shellView = serviceLocator.GetInstance(shellViewType) as FrameworkElement;
            var shellWindow = shellView as Window ?? new Window {Content = shellView};
            shellWindow.DataContext = shellViewModel;
            shellWindow.Show();
        }
    }
}
