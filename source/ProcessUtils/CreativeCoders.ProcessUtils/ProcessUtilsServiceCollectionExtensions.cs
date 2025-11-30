using CreativeCoders.ProcessUtils.Execution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.ProcessUtils;

public static class ProcessUtilsServiceCollectionExtensions
{
    public static void AddProcessUtils(this IServiceCollection services)
    {
        services.TryAddSingleton<IProcessFactory, DefaultProcessFactory>();

        services.TryAddTransient<IProcessExecutorBuilder, ProcessExecutorBuilder>();

        services.TryAddTransient(typeof(IProcessExecutorBuilder<>), typeof(ProcessExecutorBuilder<>));
    }
}
