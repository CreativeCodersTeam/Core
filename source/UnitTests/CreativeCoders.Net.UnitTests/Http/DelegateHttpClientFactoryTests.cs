using System.Net.Http;
using CreativeCoders.Net.Http;
using Xunit;

namespace CreativeCoders.Net.UnitTests.Http
{
    public class DelegateHttpClientFactoryTests
    {
        [Fact]
        public void Test()
        {
            var expectedHttpClient = new HttpClient();

            var httpClientFactory = new DelegateHttpClientFactory(name => expectedHttpClient);

            var httpClient = httpClientFactory.CreateClient(string.Empty);

            Assert.Same(expectedHttpClient, httpClient);
        }
    }
}