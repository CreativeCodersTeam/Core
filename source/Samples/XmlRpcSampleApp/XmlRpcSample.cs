using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.DynamicCode.Proxying;
using CreativeCoders.Net.Http;
using CreativeCoders.Net.Servers.Http.AspNetCore;
using CreativeCoders.Net.XmlRpc.Definition;
using CreativeCoders.Net.XmlRpc.Proxy;
using CreativeCoders.Net.XmlRpc.Server;
using JetBrains.Annotations;

namespace XmlRpcSampleApp
{
    public class XmlRpcSample
    {
        public async Task RunAsync()
        {
            var xmlRpcServer = new XmlRpcServer(new AspNetCoreHttpServer(), new XmlRpcServerMethods(), Encoding.UTF8);
            xmlRpcServer.Urls.Add("http://localhost:12345/");
            xmlRpcServer.Methods.RegisterMethods(this);

            await xmlRpcServer.StartAsync();
            
            var xmlRpcClient = new XmlRpcProxyBuilder<ISampleXmlRpcClient>(new ProxyBuilder<ISampleXmlRpcClient>(), new DelegateClassFactory<IHttpClient>(() => new HttpClientEx(new HttpClient())))
                .ForUrl("http://localhost:12345")
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
}