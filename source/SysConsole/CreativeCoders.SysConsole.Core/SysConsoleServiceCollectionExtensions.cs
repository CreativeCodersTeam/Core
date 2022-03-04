using CreativeCoders.SysConsole.Core.Abstractions;
using CreativeCoders.SysConsole.Core.Default;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    [PublicAPI]
    public static class SysConsoleServiceCollectionExtensions
    {
        public static IServiceCollection AddSysConsole(this IServiceCollection services)
        {
            services.TryAddSingleton<ISysConsole, DefaultSysConsole>();

            return services;
        }
    }
}
