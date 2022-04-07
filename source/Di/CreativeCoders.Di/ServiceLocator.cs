using System;
using System.Collections.Generic;
using CreativeCoders.Core;
using CreativeCoders.Di.Exceptions;

namespace CreativeCoders.Di;

public static class ServiceLocator
{
    private static bool IsInitialized;

    private static IDiContainer DiContainer = new NoDiContainer();

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    [Obsolete("Just for unit testing")]
    public static void XRemoveDiContainerForUnitTests()
    {
        DiContainer = new NoDiContainer();
        IsInitialized = false;
    }

    public static void Init(Func<IDiContainer> getDiContainer)
    {
        Ensure.IsNotNull(getDiContainer, nameof(getDiContainer));

        if (IsInitialized)
        {
            throw new ServiceLocatorAlreadyInitializedException();
        }

        DiContainer = getDiContainer();
        IsInitialized = true;
    }

    public static T GetInstance<T>()
        where T : class
    {
        return DiContainer.GetInstance<T>();
    }

    public static object GetInstance(Type serviceType)
    {
        return DiContainer.GetInstance(serviceType);
    }

    public static IEnumerable<T> GetInstances<T>()
        where T : class
    {
        return DiContainer.GetInstances<T>();
    }

    public static IEnumerable<object> GetInstances(Type serviceType)
    {
        return DiContainer.GetInstances(serviceType);
    }
}
