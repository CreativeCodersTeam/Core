using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Net.Http;

namespace CreativeCoders.UnitTests.Net.Http;

/// <summary>   A context for a mocked HTTP client. </summary>
public class MockHttpClientContext
{
    private readonly IList<MockHttpResponder> _responders;

    private readonly List<RecordedHttpRequest> _recordedRequests;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="MockHttpClientContext"/> class.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public MockHttpClientContext()
    {
        _responders = new List<MockHttpResponder>();
        _recordedRequests = new List<RecordedHttpRequest>();
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Creates a new <see cref="IMockHttpResponder"/>. </summary>
    ///
    /// <returns>   An IMockHttpResponder. </returns>
    ///-------------------------------------------------------------------------------------------------
    public IMockHttpResponder Respond()
    {
        var responder = new MockHttpResponder();

        _responders.Add(responder);

        return responder;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Creates a message handler for <see cref="HttpClient"/>. </summary>
    ///
    /// <returns>   The new HTTP message handler. </returns>
    ///-------------------------------------------------------------------------------------------------
    public HttpMessageHandler CreateMessageHandler()
    {
        return new DelegateHttpMessageHandler(SendAsync);
    }

    private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage,
        CancellationToken cancellationToken)
    {
        var recordedRequest = new RecordedHttpRequest(requestMessage, cancellationToken);
        await recordedRequest.CloneContent();

        _recordedRequests.Add(recordedRequest);

        foreach (var responder in _responders)
        {
            var response = await responder.Execute(requestMessage, cancellationToken).ConfigureAwait(false);

            if (response != null)
            {
                return response;
            }
        }

        throw new NoResponderFoundException();
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the recorded requests. </summary>
    ///
    /// <value> The recorded requests. </value>
    ///-------------------------------------------------------------------------------------------------
    public IReadOnlyCollection<RecordedHttpRequest> RecordedRequests => _recordedRequests;
}