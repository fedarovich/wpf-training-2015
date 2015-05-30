using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
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
