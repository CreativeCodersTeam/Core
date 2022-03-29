using Castle.DynamicProxy;

namespace CreativeCoders.DynamicCode.Proxying;

public class ProxyBuilder<T> : IProxyBuilder<T>
    where T : class
{
    public T Build(InterceptorBase<T> interceptor)
    {
        var proxyGenerator = new ProxyGenerator();
        var proxy = proxyGenerator.CreateInterfaceProxyWithoutTarget<T>(interceptor);

        return proxy;
    }
}
