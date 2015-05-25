using System;

namespace DemoMvvm
{
    public class ViewResolver : IViewResolver
    {
        public Type Resolve(Type viewModelType)
        {
            var name = viewModelType.FullName.Replace("ViewModel", "View");
            var assembly = viewModelType.Assembly;
            var viewType = assembly.GetType(name, false);
            return viewType;
        }
    }
}
