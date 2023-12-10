using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Core.SysEnvironment;

[ExcludeFromCodeCoverage]
[PublicAPI]
public static class EnvServiceCollectionExtensions
{
    public static void AddEnvironment(this IServiceCollection services)
    {
        services.TryAddSingleton<IEnvironment, EnvironmentWrapper>();
    }
}
