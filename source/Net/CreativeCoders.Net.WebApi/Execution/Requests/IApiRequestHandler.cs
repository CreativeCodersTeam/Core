using CreativeCoders.Net.WebApi.Specification;

namespace CreativeCoders.Net.WebApi.Execution.Requests;

internal interface IApiRequestHandler
{
    ApiMethodReturnType MethodReturnType { get; }

    object SendRequest(RequestData requestData);
}
