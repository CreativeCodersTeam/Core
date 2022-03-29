using System;
using System.Linq;
using Castle.DynamicProxy;
using CreativeCoders.DynamicCode.Proxying;
using CreativeCoders.Net.WebApi.Specification;

namespace CreativeCoders.Net.WebApi.Execution;

public class ApiInvocator<T> : InterceptorWithPropertiesBase<T>
    where T : class
{
    private readonly ApiData _apiData;

    private readonly ApiStructure _apiStructure;

    private readonly ApiMethodExecutor _methodExecutor;

    public ApiInvocator(ApiData apiData, ApiStructure apiStructure)
    {
        _apiData = apiData;
        _apiStructure = apiStructure;
        _methodExecutor = new ApiMethodExecutor(apiStructure, apiData.HttpClient);
    }

    protected override void ExecuteMethod(IInvocation invocation)
    {
        var apiMethod = _apiStructure.MethodInfos.FirstOrDefault(m => m.Method == invocation.Method);

        if (apiMethod == null)
        {
            throw new NotImplementedException();
        }

        var result = _methodExecutor.Invoke(invocation.Proxy, apiMethod, invocation.Arguments, _apiData);

        if (invocation.Method.ReturnType != typeof(void))
        {
            invocation.ReturnValue = result;
        }
    }
}