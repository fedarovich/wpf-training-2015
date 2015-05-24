using System;

namespace DemoMvvm
{
    public interface IViewModelResolver
    {
        Type Resolve(Type viewType);
    }
}