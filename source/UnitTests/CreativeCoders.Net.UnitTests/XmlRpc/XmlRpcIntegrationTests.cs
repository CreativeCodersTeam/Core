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
    public async Task Test()
    {
        const string expectedText = "HelloWorld";

        var xmlRpcDemoService = new XmlRpcDemoService();

        using var xmlRpcServer = new XmlRpcServer(new AspNetCoreHttpServer());
        xmlRpcServer.Urls.Add("http://localhost:12345/");
        xmlRpcServer.Methods.RegisterMethods(xmlRpcDemoService);

        await xmlRpcServer.StartAsync();

        var services = new ServiceCollection();

        services.AddXmlRpc();

        var sp = services.BuildServiceProvider();

        var xmlRpcClient = sp.GetRequiredService<IXmlRpcProxyBuilder<IXmlRpcDemoClient>>()
            .ForUrl("http://localhost:12345")
            .Build();

        // Act
        await xmlRpcClient.DoSomethingAsync(expectedText).ConfigureAwait(false);

        await xmlRpcServer.StopAsync().ConfigureAwait(false);

        // Assert
        xmlRpcDemoService.Text
            .Should()
            .Be(expectedText);
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

    public string Text { get; set; }
}

public interface IXmlRpcDemoClient
{
    [XmlRpcMethod(nameof(XmlRpcDemoService.DoSomething))]
    Task DoSomethingAsync(string text);
}
