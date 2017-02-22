using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Unity;
using PrismDemo.Services;
using PrismDemo.Services.Implementation;
using PrismDemo.Views;

namespace PrismDemo
{
    public class Bootstrapper : UnityBootstrapper
    {
        public Bootstrapper() : base()
        {
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            var shell = (Window) Shell;
            Application.Current.MainWindow = shell;
            shell.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<IDialogService, DialogService>();

            // Use the container when resolving view models by convention.
            ViewModelLocationProvider.SetDefaultViewModelFactory(type => Container.Resolve(type));
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return Prism.Modularity.ModuleCatalog.CreateFromXaml(
                new Uri("/ModuleCatalog.xaml", UriKind.Relative));
        }
    }
}
