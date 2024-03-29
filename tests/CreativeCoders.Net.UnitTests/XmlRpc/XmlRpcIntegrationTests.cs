﻿using System;
using System.Diagnostics.CodeAnalysis;
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

        var (server, demoService, client) = await CreateXmlRpcServerAndClient();

        // Act
        await client.DoSomethingAsync(expectedText);

        await server.StopAsync();
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

        var (server, _, client) = await CreateXmlRpcServerAndClient();

        // Act
        var actualText = await client.DoubleTextAsync(text, index);

        await server.StopAsync();
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
            .ForUrl(new Uri("http://localhost:12345"))
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
    [SuppressMessage("Performance", "CA1822")]
    public Task<string> DoubleText(string text, int index)
    {
        return Task.FromResult($"{text}{text}{index}");
    }

    public string Text { get; private set; }
}

public interface IXmlRpcDemoClient
{
    [XmlRpcMethod(nameof(XmlRpcDemoService.DoSomething))]
    Task DoSomethingAsync(string text);

    [XmlRpcMethod(nameof(XmlRpcDemoService.DoubleText))]
    Task<string> DoubleTextAsync(string text, int index);
}
