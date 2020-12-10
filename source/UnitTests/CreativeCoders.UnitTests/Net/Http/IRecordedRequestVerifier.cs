using System;
using System.Net.Http;
using System.Threading;
using JetBrains.Annotations;

namespace CreativeCoders.UnitTests.Net.Http
{
    /// <summary>   Interface for verifications of recorded requests. </summary>
    [PublicAPI]
    public interface IRecordedRequestVerifier
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Checks if any request is of method <paramref name="httpMethod"/>. </summary>
        ///
        /// <param name="httpMethod">   The HTTP method to check. </param>
        ///
        /// <returns>   This instance. </returns>
        ///-------------------------------------------------------------------------------------------------
        IRecordedRequestVerifier WithVerb(HttpMethod httpMethod);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Checks if any request is of content type <paramref name="contentType"/>. </summary>
        ///
        /// <param name="contentType">  The content type to check. </param>
        ///
        /// <returns>   This instance. </returns>
        ///-------------------------------------------------------------------------------------------------
        IRecordedRequestVerifier WithContentType(string contentType);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Checks if any request is of content text <paramref name="content"/>. </summary>
        ///
        /// <param name="content">  The content to check. </param>
        ///
        /// <returns>   An IRecordedRequestVerifier. </returns>
        ///-------------------------------------------------------------------------------------------------
        IRecordedRequestVerifier WithContentText(string content);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Checks if any request meets <paramref name="verifyRequestMessage"/>. </summary>
        ///
        /// <param name="verifyRequestMessage"> Function for verifying the requests. </param>
        /// <param name="verificationInfoText"> The verification information text. </param>
        ///
        /// <returns>   This instance. </returns>
        ///-------------------------------------------------------------------------------------------------
        IRecordedRequestVerifier RequestMeets(Func<HttpRequestMessage, bool> verifyRequestMessage,
            string verificationInfoText);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Checks if any request meets <paramref name="verifyRequestMessage"/>. </summary>
        ///
        /// <param name="verifyRequestMessage"> Function for verifying the requests. </param>
        /// <param name="verificationInfoText"> The verification information text. </param>
        ///
        /// <returns>   This instance. </returns>
        ///-------------------------------------------------------------------------------------------------
        IRecordedRequestVerifier RequestMeets(Func<HttpRequestMessage, CancellationToken, bool> verifyRequestMessage,
            string verificationInfoText);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Checks if the requests count is equal to <paramref name="count"/>. </summary>
        ///
        /// <param name="count">    Number of. </param>
        ///
        /// <returns>   An IRecordedRequestVerifier. </returns>
        ///-------------------------------------------------------------------------------------------------
        IRecordedRequestVerifier RequestCount(int count);
    }
}