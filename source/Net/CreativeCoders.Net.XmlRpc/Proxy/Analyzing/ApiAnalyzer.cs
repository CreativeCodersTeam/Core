using System;
using System.Linq;
using System.Reflection;
using CreativeCoders.Net.XmlRpc.Definition;
using CreativeCoders.Net.XmlRpc.Proxy.Specification;

namespace CreativeCoders.Net.XmlRpc.Proxy.Analyzing;

public class ApiAnalyzer<T>
{
    public ApiStructure Analyze()
    {
        var apiType = typeof(T);
        var methods = apiType.GetMethods();

        var exceptionHandler = GetExceptionHandler();
            
        return new ApiStructure
        {
            MethodInfos = methods.Select(method => new ApiMethodAnalyzer(method, exceptionHandler).Analyze())
        };
    }

    private static IMethodExceptionHandler GetExceptionHandler()
    {
        var exceptionHandlerAttr = typeof(T).GetCustomAttribute<GlobalExceptionHandlerAttribute>();

        return exceptionHandlerAttr != null
            ? Activator.CreateInstance(exceptionHandlerAttr.ExceptionHandler) as IMethodExceptionHandler
            : null;
    }
}