using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorWebAssemblySampleApp.ViewModels;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorWebAssemblySampleApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("app");

        builder.Services.AddTransient(sp => new HttpClient
            {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
        builder.Services.AddSingleton<TestViewModel>();

        builder.Services.AddOidcAuthentication(options =>
            builder.Configuration.Bind("Local", options.ProviderOptions));

        await builder.Build().RunAsync();
    }
}
