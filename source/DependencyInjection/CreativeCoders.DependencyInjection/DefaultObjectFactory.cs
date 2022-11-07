using CreativeCoders.Core;

namespace CreativeCoders.DependencyInjection;

internal class DefaultObjectFactory<T> : IObjectFactory<T>
{
    private readonly IObjectFactory _objectFactory;

    public DefaultObjectFactory(IObjectFactory objectFactory)
    {
        _objectFactory = Ensure.NotNull(objectFactory, nameof(objectFactory));
    }

    public T GetInstance()
    {
        return _objectFactory.GetInstance<T>();
    }

    public T CreateInstance(params object[] parameters)
    {
        return _objectFactory.CreateInstance<T>(parameters);
    }
}

internal class DefaultObjectFactory : IObjectFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DefaultObjectFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = Ensure.NotNull(serviceProvider, nameof(serviceProvider));
    }

    public T GetInstance<T>()
    {
        return _serviceProvider.GetServiceOrCreateInstance<T>();
    }

    public T CreateInstance<T>(params object[] parameters)
    {
        return _serviceProvider.CreateInstance<T>(parameters);
    }
}
