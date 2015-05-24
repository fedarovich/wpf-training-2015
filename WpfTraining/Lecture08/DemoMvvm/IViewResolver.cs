using System;

namespace DemoMvvm
{
    public interface IViewResolver
    {
        Type Resolve(Type viewModelType);
    }
}