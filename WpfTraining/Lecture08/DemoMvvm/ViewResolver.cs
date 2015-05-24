using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMvvm
{
    public class ViewResolver : IViewResolver
    {
        public Type Resolve(Type viewModelType)
        {
            var name = viewModelType.FullName.Replace("ViewModel", "View");
            var viewType = Type.GetType(name, false);
            return viewType;
        }
    }
}
