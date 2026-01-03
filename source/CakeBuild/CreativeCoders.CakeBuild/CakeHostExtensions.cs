using System.Reflection;
using Cake.Frosting;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.CakeBuild;

[PublicAPI]
public static class CakeHostExtensions
{
    public static CakeHost AddTask<TTask>(this CakeHost host)
        where TTask : class, IFrostingTask
    {
        return host.ConfigureServices(x => x.AddSingleton<IFrostingTask, TTask>());
    }

    public static CakeHost AddTask<TTask, TTaskSettings>(this CakeHost host, TTaskSettings taskSettings)
        where TTask : class, IFrostingTask
        where TTaskSettings : class
    {
        ArgumentNullException.ThrowIfNull(host);

        return host.ConfigureServices(x =>
            {
                x.AddSingleton<IFrostingTask, TTask>();

                x.TryAddSingleton(taskSettings);
            }
        );
    }

    public static CakeHost AddTasks(this CakeHost host, params Type[] taskTypes)
    {
        ArgumentNullException.ThrowIfNull(host);
        ArgumentNullException.ThrowIfNull(taskTypes);

        return host.ConfigureServices(x =>
        {
            foreach (var taskType in taskTypes)
            {
                if (!typeof(IFrostingTask).IsAssignableFrom(taskType))
                {
                    throw new InvalidOperationException(
                        $"Type '{taskType.FullName}' does not implement '{nameof(IFrostingTask)}'.");
                }

                x.AddSingleton(typeof(IFrostingTask), taskType);
            }
        });
    }

    public static CakeHost UseBuildSetup<TBuildSetup>(this CakeHost host)
        where TBuildSetup : class
    {
        host.ConfigureServices(services => services.AddSingleton<TBuildSetup>());

        foreach (var propertyInfo in typeof(TBuildSetup).GetProperties(BindingFlags.Public |
                                                                       BindingFlags.Instance))
        {
            host.ConfigureServices(services => services.AddSingleton(propertyInfo.PropertyType, sp =>
            {
                var buildSetup = sp.GetRequiredService<TBuildSetup>();

                return propertyInfo.GetValue(buildSetup) ?? throw new InvalidOperationException(
                    $"Property '{propertyInfo.Name}' of BuildSetup '{typeof(TBuildSetup).FullName}' is null.");
            }));
        }

        return host;
    }
}
