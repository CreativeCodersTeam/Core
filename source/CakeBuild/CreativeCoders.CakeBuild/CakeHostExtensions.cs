using System;
using System.Reflection;
using Cake.Frosting;
using CreativeCoders.CakeBuild.BuildServer;
using CreativeCoders.CakeBuild.Tasks.Defaults;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
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
        ArgumentNullException.ThrowIfNull(host);

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

    public static CakeHost AddDefaultTasks(this CakeHost host)
    {
        return host.AddTasks(
            typeof(CleanTask),
            typeof(RestoreTask),
            typeof(BuildTask),
            typeof(TestTask),
            typeof(CodeCoverageTask),
            typeof(PackTask),
            typeof(NuGetPublishTask));
    }

    public static CakeHost AddBuildServerIntegration(this CakeHost host)
    {
        return host
            .UseTaskSetup<StartGroupTaskSetup>()
            .UseTaskTeardown<EndGroupTaskTeardown>();
    }

    public static CakeHost SetupHost<TBuildContext, TBuildSetup>(this CakeHost host)
        where TBuildSetup : class
        where TBuildContext : BuildContext
    {
        return host
            .UseContext<TBuildContext>()
            .UseBuildSetup<TBuildSetup>();
    }

    public static CakeHost UseBuildSetup<TBuildSetup>(this CakeHost host)
        where TBuildSetup : class
    {
        host.ConfigureServices(services =>
        {
            services.AddSingleton<TBuildSetup>();
            services.AddSettingsInterfacesFor(typeof(TBuildSetup), typeof(TBuildSetup));
        });

        return host;
    }

    private static void AddSettingsInterfacesFor(this IServiceCollection services, Type type,
        Type buildSetupType)
    {
        var interfaceTypes = type.GetInterfaces();

        foreach (var interfaceType in interfaceTypes)
        {
            if (interfaceType.GetCustomAttribute<CakeTaskSettingsAttribute>() is not null)
            {
                services.AddSingleton(interfaceType, sp => sp.GetRequiredService(buildSetupType));
            }

            services.AddSettingsInterfacesFor(interfaceType, buildSetupType);
        }
    }
}
