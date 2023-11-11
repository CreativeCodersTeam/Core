using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Http;

///-------------------------------------------------------------------------------------------------
/// <summary>
///     <see cref="HttpClient"/> extension methods for a POST request sending a string.
/// </summary>
///-------------------------------------------------------------------------------------------------
[PublicAPI]
public static class HttpClientPostTextExtensions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A HttpClient extension method that posts a string asynchronous. </summary>
    ///
    /// <param name="httpClient">           The httpClient to act on. </param>
    /// <param name="requestUri">           URI of the request. </param>
    /// <param name="content">              The content. </param>
    /// <param name="mediaType">            Type of the media. </param>
    /// <param name="completionOption">     The completion option. </param>
    /// <param name="cancellationToken">    A token that allows processing to be cancelled. </param>
    ///
    /// <returns>   The task object representing the asynchronous operation. </returns>
    ///-------------------------------------------------------------------------------------------------
    public static async Task<HttpResponseMessage> PostTextAsync(this HttpClient httpClient, Uri requestUri,
        string content, string mediaType, HttpCompletionOption completionOption,
        CancellationToken cancellationToken)
    {
        using var httpPostRequest = new HttpRequestMessage(HttpMethod.Post, requestUri);

        httpPostRequest.Content = new StringContent(content, Encoding.UTF8, mediaType);

        var response = await httpClient
            .SendAsync(httpPostRequest, completionOption, cancellationToken)
            .ConfigureAwait(false);

        return response;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A HttpClient extension method that posts a string asynchronous. </summary>
    ///
    /// <param name="httpClient">           The httpClient to act on. </param>
    /// <param name="requestUri">           URI of the request. </param>
    /// <param name="content">              The content. </param>
    /// <param name="mediaType">            (Optional) Type of the media. </param>
    /// <param name="cancellationToken">    (Optional) A token that allows processing to be
    ///                                     cancelled. </param>
    ///
    /// <returns>   The task object representing the asynchronous operation. </returns>
    ///-------------------------------------------------------------------------------------------------
    public static Task<HttpResponseMessage> PostTextAsync(this HttpClient httpClient, Uri requestUri,
        string content, string mediaType = ContentMediaTypes.Text.Plain,
        CancellationToken cancellationToken = default)
    {
        return httpClient.PostTextAsync(requestUri, content, mediaType,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);
    }
}
