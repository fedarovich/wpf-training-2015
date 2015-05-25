using System.Windows;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;

namespace DemoMvvm.Autofac
{
    public class AutofacBootstrapperBase<TShellViewModel> : BootstrapperBase<TShellViewModel>
    {
        public AutofacBootstrapperBase(Application application) : base(application)
        {
        }

        protected sealed override void ConfigureIoC()
        {
            var builder = new ContainerBuilder();
            ConfigureContainer(builder);
            Container = builder.Build();
            var locator = new AutofacServiceLocator(Container);
            ServiceLocator.SetLocatorProvider(() => locator);
        }

        protected virtual void ConfigureContainer(ContainerBuilder builder)
        {
        }

        protected IContainer Container { get; private set; }
    }
}
