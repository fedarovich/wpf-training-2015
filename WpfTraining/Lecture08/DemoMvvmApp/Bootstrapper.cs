using System.Reflection;
using System.Windows;
using Autofac;
using DemoMvvm;
using DemoMvvm.Autofac;
using DemoMvvm.Services.Implementation;
using DemoMvvmApp.ViewModels;

namespace DemoMvvmApp
{
    public class Bootstrapper : AutofacBootstrapperBase<ShellViewModel>
    {
        public Bootstrapper(Application application) : base(application)
        {
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);

            builder.RegisterType<ViewResolver>()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterType<ViewModelResolver>()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterType<EventAggregator>()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterType<DialogService>()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterType<WindowManager>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.Name.EndsWith("View"))
                .AsSelf()
                .InstancePerDependency();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.Name.EndsWith("ViewModel"))
                .AsSelf()
                .InstancePerDependency();
        }
    }
}
