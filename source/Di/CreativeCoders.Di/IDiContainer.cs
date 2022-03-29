using System;
using System.Collections.Generic;

namespace CreativeCoders.Di;

public interface IDiContainer : IServiceProvider
{
    T GetInstance<T>()
        where T : class;

    object GetInstance(Type serviceType);

    T GetInstance<T>(string name)
        where T : class;

    object GetInstance(Type serviceType, string name);

    IEnumerable<T> GetInstances<T>()
        where T : class;

    IEnumerable<object> GetInstances(Type serviceType);

    bool TryGetInstance<T>(out T instance)
        where T : class;

    bool TryGetInstance(Type serviceType, out object instance);

    IDiContainerScope CreateScope();
}