using System.Threading.Tasks;
using CreativeCoders.Net.Servers.Http.AspNetCore;
using CreativeCoders.Net.XmlRpc;
using CreativeCoders.Net.XmlRpc.Definition;
using CreativeCoders.Net.XmlRpc.Proxy;
using CreativeCoders.Net.XmlRpc.Server;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc;

public class XmlRpcIntegrationTests
{
    [Fact]
    public async Task DoSomethingAsync_Call_ParameterIsPassedToService()
    {
        const string expectedText = "HelloWorld";

        var (server, demoService, client) = await CreateXmlRpcServerAndClient().ConfigureAwait(false);

        // Act
        await client.DoSomethingAsync(expectedText).ConfigureAwait(false);

        await server.StopAsync().ConfigureAwait(false);
        server.Dispose();

        // Assert
        demoService.Text
            .Should()
            .Be(expectedText);
    }

    [Fact]
    public async Task DoubleTextAsync_CallWithTextAndIndex_CorrectReturnValueIsReturned()
    {
        const string expectedReturnValue = "abcdabcd1234";
        const string text = "abcd";
        const int index = 1234;

        var (server, demoService, client) = await CreateXmlRpcServerAndClient().ConfigureAwait(false);

        // Act
        var actualText = await client.DoubleTextAsync(text, index).ConfigureAwait(false);

        await server.StopAsync().ConfigureAwait(false);
        server.Dispose();

        // Assert
        actualText
            .Should()
            .Be(expectedReturnValue);
    }

    private static async Task<(XmlRpcServer server, XmlRpcDemoService demoService, IXmlRpcDemoClient client)> CreateXmlRpcServerAndClient()
    {
        var xmlRpcDemoService = new XmlRpcDemoService();

        var xmlRpcServer = new XmlRpcServer(new AspNetCoreHttpServer(), true);
        xmlRpcServer.Urls.Add("http://localhost:12345/");
        xmlRpcServer.Methods.RegisterMethods(xmlRpcDemoService);

        await xmlRpcServer.StartAsync();

        var services = new ServiceCollection();

        services.AddXmlRpc();

        var sp = services.BuildServiceProvider();

        var xmlRpcClient = sp.GetRequiredService<IXmlRpcProxyBuilder<IXmlRpcDemoClient>>()
            .ForUrl("http://localhost:12345")
            .Build();

        return (xmlRpcServer, xmlRpcDemoService, xmlRpcClient);
    }
}

public class XmlRpcDemoService
{
    [XmlRpcMethod]
    public Task DoSomething(string text)
    {
        Text = text;

        return Task.CompletedTask;
    }

    [XmlRpcMethod]
    public Task<string> DoubleText(string text, int index)
    {
        return Task.FromResult($"{text}{text}{index}");
    }

    public string Text { get; set; }
}

public interface IXmlRpcDemoClient
{
    [XmlRpcMethod(nameof(XmlRpcDemoService.DoSomething))]
    Task DoSomethingAsync(string text);

    [XmlRpcMethod(nameof(XmlRpcDemoService.DoubleText))]
    Task<string> DoubleTextAsync(string text, int index);
}
