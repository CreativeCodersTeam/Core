using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.Core.Reflection;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Model.Values.Converters;
using CreativeCoders.Net.XmlRpc.Proxy.Specification;

namespace CreativeCoders.Net.XmlRpc.Proxy.Execution.Requests;

public class DictionaryRequestHandler : XmlRpcRequestHandlerBase
{
    public DictionaryRequestHandler() : base(ApiMethodReturnType.Dictionary)
    {
    }

    public override object HandleRequest(RequestData requestData)
    {
        return this.ExecuteGenericMethod<object>(nameof(GetDictionaryAsync), new[] { requestData.ValueType }, requestData);
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private async Task<IDictionary<string, T>> GetDictionaryAsync<T>(RequestData requestData)
    {
        return await GetValueWithExceptionHandlingAsync(
            () => GetDictionaryInternalAsync<T>(requestData),
            requestData).ConfigureAwait(false);
    }
        
    private async Task<IDictionary<string, T>> GetDictionaryInternalAsync<T>(RequestData requestData)
    {
        var xmlRpcResponse = await requestData.Client.InvokeExAsync(requestData.MethodName, requestData.Arguments).ConfigureAwait(false);

        if (xmlRpcResponse.Results.First().Values.First() is not StructValue xmlRpcValue)
        {
            throw new InvalidOperationException("Xml rpc result must be a struct");
        }

        var value = xmlRpcValue.Value.ToDictionary(x => x.Key, x => GetItem<T>(x.Value));

        return value;
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private T GetItem<T>(XmlRpcValue xmlRpcValue)
    {
        var xmlRpcValueToDataConverter = new XmlRpcValueToDataConverter();

        if (typeof(T) == typeof(XmlRpcValue))
        {
            return (T) (object) xmlRpcValue;
        }

        return xmlRpcValueToDataConverter.Convert<T>(xmlRpcValue);
    }
}