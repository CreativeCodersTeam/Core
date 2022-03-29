using System;
using System.Threading.Tasks;
using CreativeCoders.Net.XmlRpc.Definition;
using CreativeCoders.Net.XmlRpc.Proxy.Specification;

namespace CreativeCoders.Net.XmlRpc.Proxy.Execution.Requests;

public abstract class XmlRpcRequestHandlerBase : IXmlRpcApiRequestHandler
{
    protected XmlRpcRequestHandlerBase(ApiMethodReturnType returnType)
    {
        ReturnType = returnType;
    }

    public abstract object HandleRequest(RequestData requestData);

    protected static async Task<T> GetValueWithExceptionHandlingAsync<T>(Func<Task<T>> execute,
        RequestData requestData)
    {
        try
        {
            return await execute().ConfigureAwait(false);
        }
        catch (Exception e)
        {
            if (requestData.ExceptionHandler == null)
            {
                throw;
            }

            var exceptionArguments = new MethodExceptionHandlerArguments {MethodException = e};

            requestData.ExceptionHandler.HandleException(exceptionArguments);

            if (exceptionArguments.Handled)
            {
                return (T) requestData.DefaultResult;
            }

            throw exceptionArguments.MethodException ?? e;
        }
    }

    protected static async Task ExecuteWithExceptionHandlingAsync(Func<Task> execute, RequestData requestData)
    {
        try
        {
            await execute().ConfigureAwait(false);
        }
        catch (Exception e)
        {
            if (requestData.ExceptionHandler == null)
            {
                throw;
            }

            var exceptionArguments = new MethodExceptionHandlerArguments {MethodException = e};

            requestData.ExceptionHandler.HandleException(exceptionArguments);

            if (exceptionArguments.Handled)
            {
                return;
            }

            throw exceptionArguments.MethodException ?? e;
        }
    }

    public ApiMethodReturnType ReturnType { get; }
}
