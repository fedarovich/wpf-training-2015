using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMvvm
{
    public class ViewModelResolver : IViewModelResolver
    {
        public Type Resolve(Type viewType)
        {
            var name = viewType.FullName.Replace("View", "ViewModel");
            var viewModelType = Type.GetType(name, false);
            return viewModelType;
        }
    }
}
