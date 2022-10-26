using System;
using Castle.DynamicProxy;
using CreativeCoders.Core;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.DynamicCode.Proxying;

public class ProxyBuilderFactory : IProxyBuilderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ProxyBuilderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = Ensure.NotNull(serviceProvider, nameof(serviceProvider));
    }

    public IProxyBuilder<T> Create<T>() where T : class
    {
        return new ProxyBuilder<T>(_serviceProvider.GetRequiredService<IProxyGenerator>());
    }
}
