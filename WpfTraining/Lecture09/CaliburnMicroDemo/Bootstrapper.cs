using System.Reflection;
using System.Windows;
using Autofac;
using Caliburn.Micro.Autofac;
using Caliburn.Micro.Extras;
using CaliburnMicroDemo.ViewModels;

namespace CaliburnMicroDemo
{
    public class Bootstrapper : AutofacBootstrapper<ShellViewModel>
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<MessageService>()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterType<SaveFileService>()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterType<OpenFileService>()
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

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
