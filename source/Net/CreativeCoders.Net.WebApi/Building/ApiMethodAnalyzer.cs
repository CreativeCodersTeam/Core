using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using CreativeCoders.Net.WebApi.Definition;
using CreativeCoders.Net.WebApi.Exceptions;
using CreativeCoders.Net.WebApi.Specification;

namespace CreativeCoders.Net.WebApi.Building;

internal class ApiMethodAnalyzer
{
    private readonly MethodInfo _methodInfo;

    private readonly ApiMethodBaseAttribute _apiMethodAttribute;

    public ApiMethodAnalyzer(MethodInfo methodInfo, ApiMethodBaseAttribute apiMethodAttribute)
    {
        _methodInfo = methodInfo;
        _apiMethodAttribute = apiMethodAttribute;
    }

    public ApiMethodInfo GetMethodInfo()
    {
        var methodReturnType = GetMethodReturnType();
        var headerDefinitions = GetHeaderDefinitions().ToArray();

        var apiMethodInfo = new ApiMethodInfo
        {
            Method = _methodInfo,
            ReturnType = methodReturnType,
            RequestMethod = _apiMethodAttribute.RequestMethod,
            HeaderDefinitions = headerDefinitions,
            MethodUri = _apiMethodAttribute.Uri,
            DefaultCompletionOption =
                _apiMethodAttribute.GetCompletionOption(HttpCompletionOption.ResponseContentRead)
        };

        apiMethodInfo.ArgumentInfos =
            new ApiCallArgumentsAnalyzer(apiMethodInfo).Analyze(_methodInfo).ToArray();

        return apiMethodInfo;
    }

    private IEnumerable<RequestHeader> GetHeaderDefinitions()
    {
        return
            _methodInfo
                .GetCustomAttributes<HeaderAttribute>()
                .Select(a => new RequestHeader(a.Name, a.Value));
    }

    private ApiMethodReturnType GetMethodReturnType()
    {
        if (_methodInfo.ReturnType.IsGenericType &&
            _methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            var genericArgument = _methodInfo.ReturnType.GetGenericArguments().First();

            if (genericArgument == typeof(HttpResponseMessage))
            {
                return ApiMethodReturnType.HttpResponseMessage;
            }

            if (genericArgument == typeof(string))
            {
                return ApiMethodReturnType.String;
            }

            if (genericArgument == typeof(Stream))
            {
                return ApiMethodReturnType.Stream;
            }

            if (genericArgument.IsGenericType &&
                genericArgument.GetGenericTypeDefinition() == typeof(Response<>))
            {
                return ApiMethodReturnType.Response;
            }

            if (genericArgument.IsClass || genericArgument.IsInterface)
            {
                return ApiMethodReturnType.DataObject;
            }
        }

        if (_methodInfo.ReturnType == typeof(Task))
        {
            return ApiMethodReturnType.Void;
        }

        throw new ReturnTypeNotSupportedException(_methodInfo);
    }
}
