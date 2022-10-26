using Castle.DynamicProxy;
using CreativeCoders.Core;

namespace CreativeCoders.DynamicCode.Proxying;

public class ProxyBuilder<T> : IProxyBuilder<T>
    where T : class
{
    private readonly IProxyGenerator _proxyGenerator;

    public ProxyBuilder(IProxyGenerator proxyGenerator)
    {
        _proxyGenerator = Ensure.NotNull(proxyGenerator, nameof(proxyGenerator));
    }

    public T Build(InterceptorBase<T> interceptor)
    {
        var proxy = _proxyGenerator.CreateInterfaceProxyWithoutTarget<T>(interceptor);

        return proxy;
    }
}
