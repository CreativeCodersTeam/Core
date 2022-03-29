using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Core;

namespace CreativeCoders.Net.Http;

///-------------------------------------------------------------------------------------------------
/// <summary>
///     A delegate HTTP message handler, which delegates <see cref="SendAsync"/> to a function.
/// </summary>
///
/// <seealso cref="HttpMessageHandler"/>
///-------------------------------------------------------------------------------------------------
public class DelegateHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _sendAsync;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the CreativeCoders.Net.Http.DelegateHttpMessageHandler
    ///     class.
    /// </summary>
    ///
    /// <param name="sendAsync">    The function for sending asynchronous. </param>
    ///-------------------------------------------------------------------------------------------------
    public DelegateHttpMessageHandler(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsync)
    {
        Ensure.IsNotNull(sendAsync, nameof(sendAsync));

        _sendAsync = sendAsync;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the CreativeCoders.Net.Http.DelegateHttpMessageHandler
    ///     class.
    /// </summary>
    ///
    /// <param name="sendAsync">    The function for sending asynchronous. </param>
    ///-------------------------------------------------------------------------------------------------
    public DelegateHttpMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> sendAsync)
    {
        Ensure.IsNotNull(sendAsync, nameof(sendAsync));

        _sendAsync = (request, _) => sendAsync(request);
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        return _sendAsync(request, cancellationToken);
    }
}
