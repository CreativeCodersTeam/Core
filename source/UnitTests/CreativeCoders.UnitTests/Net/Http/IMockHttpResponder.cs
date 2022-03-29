using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeCoders.UnitTests.Net.Http;

/// <summary>   Interface for specifying a mocked HTTP response. </summary>
public interface IMockHttpResponder
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Specifies URI pattern for which the responder is responsible. </summary>
    ///
    /// <param name="uriPattern">   A pattern specifying the URI. </param>
    ///
    /// <returns>   This instance. </returns>
    ///-------------------------------------------------------------------------------------------------
    IMockHttpResponder ForUri(string uriPattern);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Specifies the HTTP verb for which the responder is responsible. </summary>
    ///
    /// <param name="httpMethod">   The HTTP method. </param>
    ///
    /// <returns>   This instance. </returns>
    ///-------------------------------------------------------------------------------------------------
    IMockHttpResponder WithVerb(HttpMethod httpMethod);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Specifies the response data as text. </summary>
    ///
    /// <param name="content">      The response content. </param>
    /// <param name="statusCode">   The HTTP status code of the response. </param>
    ///
    /// <returns>   This instance. </returns>
    ///-------------------------------------------------------------------------------------------------
    IMockHttpResponder ReturnText(string content, HttpStatusCode statusCode);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Specifies the response data as text with HTTP status code 200 (OK). </summary>
    ///
    /// <param name="content">  The response content. </param>
    ///
    /// <returns>   This instance. </returns>
    ///-------------------------------------------------------------------------------------------------
    IMockHttpResponder ReturnText(string content);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Sets the function used to handle the response. </summary>
    ///
    /// <param name="requestHandler">   The request handler for the response. </param>
    ///
    /// <returns>   This instance. </returns>
    ///-------------------------------------------------------------------------------------------------
    IMockHttpResponder Return(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> requestHandler);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Specifies the response data as JSON. </summary>
    ///
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    /// <param name="data">                     The data. </param>
    /// <param name="statusCode">               The HTTP status code of the response. </param>
    /// <param name="jsonSerializerOptions">    (Optional) Options for controlling the JSON
    ///                                         serializer. </param>
    ///
    /// <returns>   This instance. </returns>
    ///-------------------------------------------------------------------------------------------------
    IMockHttpResponder ReturnJson<T>(T data, HttpStatusCode statusCode, JsonSerializerOptions jsonSerializerOptions = null);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Specifies the response data as JSON with HTTP status code 200 (OK). </summary>
    ///
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    /// <param name="data">                     The data. </param>
    /// <param name="jsonSerializerOptions">    (Optional) Options for controlling the JSON
    ///                                         serializer. </param>
    ///
    /// <returns>   This instance. </returns>
    ///-------------------------------------------------------------------------------------------------
    IMockHttpResponder ReturnJson<T>(T data, JsonSerializerOptions jsonSerializerOptions = null);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Get a new responder which is processed after this responder. </summary>
    ///
    /// <returns>   An IMockHttpResponder. </returns>
    ///-------------------------------------------------------------------------------------------------
    IMockHttpResponder Then();
}