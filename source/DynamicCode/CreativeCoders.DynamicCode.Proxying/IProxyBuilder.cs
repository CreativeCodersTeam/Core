namespace CreativeCoders.DynamicCode.Proxying;

public interface IProxyBuilder<T>
    where T : class
{
    T Build(InterceptorBase<T> interceptor);
}