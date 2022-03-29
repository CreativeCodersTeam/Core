using Castle.DynamicProxy;
using CreativeCoders.Net.XmlRpc.Proxy.Specification;

namespace CreativeCoders.Net.XmlRpc.Proxy.Execution;

public interface IApiMethodExecutor
{
    object Execute(ApiMethodInfo apiMethodInfo, IInvocation invocation);
}