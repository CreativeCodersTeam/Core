using CreativeCoders.Core;

namespace CreativeCoders.DependencyInjection;

internal class DefaultObjectFactory<T> : IObjectFactory<T>
{
    private readonly IServiceProvider _serviceProvider;

    public DefaultObjectFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = Ensure.NotNull(serviceProvider, nameof(serviceProvider));
    }

    public T GetInstance()
    {
        return _serviceProvider.GetServiceOrCreateInstance<T>();
    }

    public T CreateInstance(params object[] parameters)
    {
        return _serviceProvider.CreateInstance<T>(parameters);
    }
}
