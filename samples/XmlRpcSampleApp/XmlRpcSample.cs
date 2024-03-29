﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CreativeCoders.Net.Servers.Http;
using CreativeCoders.Net.Servers.Http.AspNetCore;
using CreativeCoders.Net.XmlRpc;
using CreativeCoders.Net.XmlRpc.Definition;
using CreativeCoders.Net.XmlRpc.Proxy;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace XmlRpcSampleApp;

[SuppressMessage("Performance", "CA1822")]
public class XmlRpcSample
{
    public async Task RunAsync()
    {
        var services = new ServiceCollection();

        services.AddXmlRpc();

        services.AddTransient<IHttpServer, AspNetCoreHttpServer>();

        var sp = services.BuildServiceProvider();

        using var xmlRpcServer = sp.GetRequiredService<IXmlRpcServerFactory>().CreateServer();

        xmlRpcServer.Urls.Add("http://localhost:12345/");
        xmlRpcServer.Methods.RegisterMethods(this);

        await xmlRpcServer.StartAsync();

        var xmlRpcClient = sp.GetRequiredService<IXmlRpcProxyBuilder<ISampleXmlRpcClient>>()
            .ForUrl(new Uri("http://localhost:12345"))
            .Build();

        var result = await xmlRpcClient.DoSomething("qwertz");

        Console.WriteLine($"Method result: {result}");

        var asyncResult = await xmlRpcClient.DoSomethingAsync("12345");

        Console.WriteLine($"Async method result: {asyncResult}");

        await xmlRpcServer.StopAsync();
    }

    [UsedImplicitly]
    [XmlRpcMethod("DoSomethingAsync")]
    private async Task<string> DoSomethingAsync(string text)
    {
        await Task.Delay(5000);
        return "Async HelloWorld" + text;
    }

    [UsedImplicitly]
    [XmlRpcMethod("DoSomething")]
    private string DoSomething(string text)
    {
        return "HelloWorld" + text;
    }
}

public interface ISampleXmlRpcClient
{
    [XmlRpcMethod]
    Task<string> DoSomethingAsync(string text);

    [XmlRpcMethod]
    Task<string> DoSomething(string text);
}
