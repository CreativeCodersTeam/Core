using System.Linq;
using System.Net.Http;
using CreativeCoders.Core;

namespace CreativeCoders.UnitTests.Net.Http
{
    public static class MockHttpClientContextExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A MockHttpClientContext extension method that creates a <see cref="HttpClient"/>. </summary>
        ///
        /// <param name="httpClientContext">    The httpClientContext to act on. </param>
        ///
        /// <returns>   The new HTTP client. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static HttpClient CreateClient(this MockHttpClientContext httpClientContext)
        {
            return new HttpClient(httpClientContext.CreateMessageHandler());
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A MockHttpClientContext extension method that call should be made. </summary>
        ///
        /// <param name="httpClientContext">    The httpClientContext to act on. </param>
        /// <param name="uriPattern">           A pattern specifying the URI. </param>
        ///
        /// <returns>   An IRecordedRequestVerifier. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IRecordedRequestVerifier CallShouldBeMade(this MockHttpClientContext httpClientContext,
            string uriPattern)
        {
            return new RecordedRequestVerifier(
                httpClientContext.RecordedRequests
                    .Where(x =>
                        PatternMatcher.MatchesPattern(x.RequestMessage.RequestUri.ToStringSafe(), uriPattern)).ToList());
        }
    }
}