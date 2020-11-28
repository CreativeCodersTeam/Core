using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeCoders.Net.Http
{
    /// <summary>   Extension methods for <see cref="HttpResponseMessage"/>. </summary>
    public static class HttpResponseMessageExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Task&lt;HttpResponseMessage&gt; extension method that reads JSON from response content
        ///     asynchronous.
        /// </summary>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="responseMessageTask">      The responseMessageTask to act on. </param>
        /// <param name="jsonSerializerOptions">    (Optional) Options for controlling the JSON
        ///                                         serializer. </param>
        /// <param name="cancellationToken">        (Optional) A token that allows processing to be
        ///                                         cancelled. </param>
        ///
        /// <returns>   The task object representing the asynchronous operation. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static async Task<T> ReceiveJsonAsync<T>(this Task<HttpResponseMessage> responseMessageTask,
            JsonSerializerOptions jsonSerializerOptions = null, CancellationToken cancellationToken = default)
        {
            using var response = await responseMessageTask.ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>(jsonSerializerOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Task&lt;HttpResponseMessage&gt; extension method that reads JSON from response content
        ///     asynchronous.
        /// </summary>
        ///
        /// <param name="responseMessageTask">      The responseMessageTask to act on. </param>
        /// <param name="resultType">               Type of the result. </param>
        /// <param name="jsonSerializerOptions">    (Optional) Options for controlling the JSON
        ///                                         serializer. </param>
        /// <param name="cancellationToken">        (Optional) A token that allows processing to be
        ///                                         cancelled. </param>
        ///
        /// <returns>   The task object representing the asynchronous operation. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static async Task<object> ReceiveJsonAsync(this Task<HttpResponseMessage> responseMessageTask,
            Type resultType, JsonSerializerOptions jsonSerializerOptions = null,
            CancellationToken cancellationToken = default)
        {
            using var response = await responseMessageTask.ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync(resultType, jsonSerializerOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Task&lt;HttpResponseMessage&gt; extension method that reads a string from response
        ///     content asynchronous.
        /// </summary>
        ///
        /// <param name="responseMessageTask">  The responseMessageTask to act on. </param>
        /// <param name="cancellationToken">    (Optional) A token that allows processing to be
        ///                                     cancelled. </param>
        ///
        /// <returns>   The task object representing the asynchronous operation. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static async Task<string> ReceiveTextAsync(this Task<HttpResponseMessage> responseMessageTask,
            CancellationToken cancellationToken = default)
        {
            using var response = await responseMessageTask.ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            //todo net5 CancellationToken
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
}