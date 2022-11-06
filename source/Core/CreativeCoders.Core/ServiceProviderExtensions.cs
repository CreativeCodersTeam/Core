using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Core;

[ExcludeFromCodeCoverage]
public static class ServiceProviderExtensions
{
    public static T GetServiceOrCreateInstance<T>(this IServiceProvider serviceProvider)
    {
        return ActivatorUtilities.GetServiceOrCreateInstance<T>(serviceProvider);
    }

    public static T CreateInstance<T>(this IServiceProvider serviceProvider, params object[] parameters)
    {
        return ActivatorUtilities.CreateInstance<T>(serviceProvider, parameters);
    }
}
