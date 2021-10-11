using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Core.Text;

namespace CreativeCoders.UnitTests.Net.Http
{
    ///<inheritdoc/>
    public class MockHttpResponder : IMockHttpResponder
    {
        private string _uriPattern;

        private HttpMethod _httpMethod;

        private Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _requestHandler;

        private MockHttpResponder _subResponder;

        private bool _isAlreadyExecuted;

        public IMockHttpResponder ForUri(string uriPattern)
        {
            _uriPattern = uriPattern;

            return this;
        }

        public IMockHttpResponder WithVerb(HttpMethod httpMethod)
        {
            _httpMethod = httpMethod;

            return this;
        }

        public IMockHttpResponder ReturnText(string content, HttpStatusCode statusCode)
        {
            return Return((_, _) => Task.FromResult(new HttpResponseMessage(statusCode)
                {Content = new StringContent(content)}));
        }

        public IMockHttpResponder ReturnText(string content)
        {
            return ReturnText(content, HttpStatusCode.OK);
        }

        public IMockHttpResponder Return(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> requestHandler)
        {
            _requestHandler = requestHandler;

            return this;
        }

        public IMockHttpResponder ReturnJson<T>(T data, HttpStatusCode statusCode, JsonSerializerOptions jsonSerializerOptions = null)
        {
            return Return((_, _) => Task.FromResult(new HttpResponseMessage(statusCode)
                { Content = JsonContent.Create(data, null, jsonSerializerOptions) }));
        }

        public IMockHttpResponder ReturnJson<T>(T data, JsonSerializerOptions jsonSerializerOptions = null)
        {
            return ReturnJson(data, HttpStatusCode.OK, jsonSerializerOptions);
        }

        public IMockHttpResponder Then()
        {
            var responder = new MockHttpResponder();

            _subResponder = responder;

            return responder;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the responder. </summary>
        ///
        /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
        ///                                                 invalid cause uri pattern or HTTP method is not
        ///                                                 matching. </exception>
        ///
        /// <param name="requestMessage">       Message describing the request. </param>
        /// <param name="cancellationToken">    A token that allows processing to be cancelled. </param>
        ///
        /// <returns>
        ///     An asynchronous result that yields the HttpResponseMessage for the request or null if
        ///     responder is not responsible for this request.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public Task<HttpResponseMessage> Execute(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            Ensure.IsNotNull(requestMessage, nameof(requestMessage));

            if (_requestHandler == null)
            {
                throw new InvalidOperationException("Return() was not configured");
            }

            if (_uriPattern.IsNotNullOrEmpty() && !PatternMatcher.MatchesPattern(requestMessage.RequestUri.ToStringSafe(), _uriPattern))
            {
                return Task.FromResult<HttpResponseMessage>(null);
            }

            if (_httpMethod != null && !requestMessage.Method.Equals(_httpMethod))
            {
                return Task.FromResult<HttpResponseMessage>(null);
            }

            if (_subResponder != null && _isAlreadyExecuted)
            {
                return _subResponder.Execute(requestMessage, cancellationToken);
            }

            _isAlreadyExecuted = true;

            return _requestHandler(requestMessage, cancellationToken);
        }
    }
}
