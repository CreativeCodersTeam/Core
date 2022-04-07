using System;
using System.Net.Http;
using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi;

[PublicAPI]
public class Response<T> : IDisposable
    where T : class
{
    private readonly Func<T> _getData;

    private T _data;

    private bool _dataIsRead;

    public Response(HttpResponseMessage responseMessage, Func<T> getData)
    {
        _getData = getData;
        ResponseMessage = responseMessage;
    }

    public HttpResponseMessage ResponseMessage { get; }

    public T Data
    {
        get
        {
            if (_dataIsRead)
            {
                return _data;
            }

            _dataIsRead = true;
            _data = _getData();

            return _data;
        }
    }

    public void Dispose()
    {
        ResponseMessage?.Dispose();

        var dataDisposable = Data as IDisposable;
        dataDisposable?.Dispose();
    }
}
