namespace CreativeCoders.DynamicCode.Proxying;

public interface IProxyBuilderFactory
{
    IProxyBuilder<T> Create<T>()
        where T : class;
}
