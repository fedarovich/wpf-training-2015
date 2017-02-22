using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using PrismDemo.Views;

namespace PrismDemo
{
    public class MainModule : IModule
    {
        private readonly IUnityContainer container;

        public MainModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
        }

        public void Initialize()
        {
            container.RegisterType<object, Document>(typeof(Document).Name);
        }
    }
}
