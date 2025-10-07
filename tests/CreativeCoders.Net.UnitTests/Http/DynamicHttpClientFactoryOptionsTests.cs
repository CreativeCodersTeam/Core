using CreativeCoders.Net.Http;
using FakeItEasy;
using AwesomeAssertions;
using Microsoft.Extensions.Http;
using Xunit;

namespace CreativeCoders.Net.UnitTests.Http;

public class DynamicHttpClientFactoryOptionsTests
{
    [Fact]
    public void Configure_GetOptionsForExistingName_OptionsAreConfigured()
    {
        HttpClientFactoryOptions configuredOptions = null;

        var httpClientSettings = A.Fake<IHttpClientSettings>();

        var namedOptions = new DynamicHttpClientFactoryOptions(httpClientSettings);

        var options = new HttpClientFactoryOptions();

        A.CallTo(() => httpClientSettings.Get("Test"))
            .Returns(Configure);

        // Act
        namedOptions.Configure("Test", options);

        // Assert
        configuredOptions
            .Should()
            .BeSameAs(options);
        return;

        void Configure(HttpClientFactoryOptions x) => configuredOptions = x;
    }
}
