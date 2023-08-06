using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Http;

[PublicAPI]
public static class HttpClientPostWithoutBodyExtensions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A <see cref="HttpClient"/> extension method for a POST request without a body
    ///     asynchronous.
    /// </summary>
    ///
    /// <param name="httpClient">           The httpClient to act on. </param>
    /// <param name="requestUri">           URI of the request. </param>
    /// <param name="completionOption">     The completion option. </param>
    /// <param name="cancellationToken">    A token that allows processing to be cancelled. </param>
    ///
    /// <returns>   The task object representing the asynchronous operation. </returns>
    ///-------------------------------------------------------------------------------------------------
    public static Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, Uri requestUri,
        HttpCompletionOption completionOption, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

        request.Content = new ByteArrayContent(Array.Empty<byte>());

        return httpClient.SendAsync(request, completionOption, cancellationToken);
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A <see cref="HttpClient"/> extension method for a POST request without a body
    ///     asynchronous.
    /// </summary>
    ///
    /// <param name="httpClient">           The httpClient to act on. </param>
    /// <param name="requestUri">           URI of the request. </param>
    /// <param name="cancellationToken">    (Optional) A token that allows processing to be
    ///                                     cancelled. </param>
    ///
    /// <returns>   The task object representing the asynchronous operation. </returns>
    ///-------------------------------------------------------------------------------------------------
    public static Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, Uri requestUri,
        CancellationToken cancellationToken = default)
    {
        return httpClient.PostAsync(requestUri, HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);
    }
}
