using System;
using System.Net.Http;
using CreativeCoders.Core;
using CreativeCoders.Net.Http;
using JetBrains.Annotations;

namespace CreativeCoders.UnitTests.Net.Http;

[PublicAPI]
public class MockHttpClientFactory : DelegateHttpClientFactory
{
    public MockHttpClientFactory(HttpMessageHandler httpMessageHandler) : base(_ =>
        new HttpClient(httpMessageHandler))
    {
        Ensure.IsNotNull(httpMessageHandler);
    }

    public MockHttpClientFactory(Func<HttpMessageHandler> createMessageHandler) : base(_ =>
        new HttpClient(createMessageHandler()))
    {
        Ensure.IsNotNull(createMessageHandler);
    }
}
