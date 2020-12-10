using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Core.IO;
using CreativeCoders.Net.XmlRpc.Definition;
using CreativeCoders.Net.XmlRpc.Exceptions;
using CreativeCoders.Net.XmlRpc.Model.Values.Converters;
using CreativeCoders.Net.XmlRpc.Reader;
using CreativeCoders.Net.XmlRpc.Reader.Values;
using CreativeCoders.Net.XmlRpc.Writer;
using CreativeCoders.Net.XmlRpc.Writer.Values;

namespace CreativeCoders.Net.XmlRpc.Client
{
    public class XmlRpcClient : IXmlRpcClient
    {
        private readonly HttpClient _httpClient;

        private readonly IRequestBuilder _requestBuilder;

        public XmlRpcClient(HttpClient httpClient)
        {
            Ensure.IsNotNull(httpClient, nameof(httpClient));
            
            _httpClient = httpClient;
            _requestBuilder = new RequestBuilder(new DataToXmlRpcValueConverter());
        }

        public async Task<XmlRpcResponse> SendRequestAsync(XmlRpcRequest request)
        {
            Ensure.IsNotNull(request, nameof(request));
            
            var httpRequest = await CreateHttpRequestAsync(request).ConfigureAwait(false);

            var httpResponse = await _httpClient
                .SendAsync(httpRequest, HttpCompletionOption.ResponseContentRead, CancellationToken.None)
                .ConfigureAwait(false);

            var response = await ReadResponseAsync(httpResponse).ConfigureAwait(false);

            if (response.Results.FirstOrDefault()?.IsFaulted == true)
            {
                throw new FaultException(response.Results.FirstOrDefault()?.FaultCode ?? 0,
                    response.Results.FirstOrDefault()?.FaultString);
            }

            return response;
        }

        private async Task<XmlRpcResponse> ReadResponseAsync(HttpResponseMessage httpResponse)
        {
            var responseContent = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            var responseModelReader = new ResponseModelReader(new ValueReaders(XmlEncoding));

            return await responseModelReader.ReadAsync(new MemoryStream(XmlEncoding.GetBytes(responseContent)), false).ConfigureAwait(false);
        }

        private async Task<HttpRequestMessage> CreateHttpRequestAsync(XmlRpcRequest request)
        {
            var contentData = await CreateContentDataAsync(request).ConfigureAwait(false);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, Url)
            {
                Content = new StringContent(contentData, XmlEncoding, HttpContentType)
            };

            return httpRequest;
        }

        private async Task<string> CreateContentDataAsync(XmlRpcRequest request)
        {
            var requestModelWriter = new RequestModelWriter(new ValueWriters());

            await using (var stream = new MemoryStream())
            {
                await requestModelWriter.WriteAsync(stream, request, XmlEncoding).ConfigureAwait(false);
                stream.Seek(0, SeekOrigin.Begin);
                return await stream.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        public async Task ExecuteAsync(string methodName, params object[] parameters)
        {
            var request = _requestBuilder.Build(methodName, parameters);

            await SendRequestAsync(request).ConfigureAwait(false);
        }

        public async Task<XmlRpcResponse> InvokeAsync(string methodName, params object[] parameters)
        {
            Ensure.IsNotNullOrWhitespace(methodName, nameof(methodName));
            
            var request = _requestBuilder.Build(methodName, parameters);

            return await SendRequestAsync(request).ConfigureAwait(false);
        }

        public async Task<T> InvokeAsync<T>(string methodName, params object[] parameters)
        {
            var response = await InvokeAsync(methodName, parameters).ConfigureAwait(false);

            var resultValue = response.Results.First().Values.First();

            return resultValue.GetValue<T>();
        }

        public Task<XmlRpcResponse> InvokeExAsync(string methodName, object[] parameters)
        {
            return InvokeAsync(methodName, parameters);
        }

        public Task<T> InvokeExAsync<T>(string methodName, object[] parameters)
        {
            return InvokeAsync<T>(methodName, parameters);
        }

        public async Task<T> InvokeExAsync<T, TInvoke>(string methodName, object[] parameters, IMethodResultConverter resultConverter)
        {
            var result = await InvokeExAsync<TInvoke>(methodName, parameters).ConfigureAwait(false);

            return (T) resultConverter.Convert(result);
        }

        public string Url { get; set; }

        public Encoding XmlEncoding
        {
            get => _requestBuilder.XmlEncoding;
            set => _requestBuilder.XmlEncoding = value;
        }

        public string HttpContentType { get; set; } = ContentMediaTypes.Application.Xml;
    }
}