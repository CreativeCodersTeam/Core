using System;
using System.Linq;
using Castle.DynamicProxy;
using CreativeCoders.Core.Reflection;
using CreativeCoders.DynamicCode.Proxying;
using CreativeCoders.Net.XmlRpc.Client;
using CreativeCoders.Net.XmlRpc.Proxy.Execution;
using CreativeCoders.Net.XmlRpc.Proxy.Specification;

namespace CreativeCoders.Net.XmlRpc.Proxy;

public class XmlRpcProxyInterceptor<T> : InterceptorWithPropertiesBase<T>
    where T : class
{
    private readonly ApiStructure _apiStructure;

    private readonly IApiMethodExecutor _apiMethodExecutor;

    public XmlRpcProxyInterceptor(IXmlRpcClient xmlRpcClient, ApiStructure apiStructure)
    {
        _apiStructure = apiStructure;

        _apiMethodExecutor = new ApiMethodExecutor(xmlRpcClient);
    }

    protected override void ExecuteMethod(IInvocation invocation)
    {
        var apiMethodInfo = _apiStructure.MethodInfos.FirstOrDefault(x => invocation.Method.MatchesMethod(x.Method));

        if (apiMethodInfo == null)
        {
            throw new InvalidOperationException();
        }

        var result = _apiMethodExecutor.Execute(apiMethodInfo, invocation);

        if (invocation.Method.ReturnType != typeof(void))
        {
            invocation.ReturnValue = result;
        }
    }
}