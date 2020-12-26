using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeCoders.UnitTests.Net.Http
{
    /// <summary>   A recorded HTTP request. </summary>
    [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
    public class RecordedHttpRequest
    {
        internal RecordedHttpRequest(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            RequestMessage = requestMessage;
            CancellationToken = cancellationToken;
        }

        internal async Task CloneContent()
        {
            var content = RequestMessage.Content;

            if (content == null)
                return;

            Stream stream = new MemoryStream();
            await content.CopyToAsync(stream).ConfigureAwait(false);
            stream.Position = 0;

            var clone = new StreamContent(stream);
            foreach (var (key, value) in content.Headers)
                clone.Headers.Add(key, value);

            Content = clone;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a message describing the recorded HTTP request. </summary>
        ///
        /// <value> A message describing the recorded HTTP request. </value>
        ///-------------------------------------------------------------------------------------------------
        public HttpRequestMessage RequestMessage { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the cancellation token of the recorded HTTP request. </summary>
        ///
        /// <value> The cancellation token of the recorded HTTP request. </value>
        ///-------------------------------------------------------------------------------------------------
        public CancellationToken CancellationToken { get; }

        public HttpContent Content { get; private set; }
    }
}