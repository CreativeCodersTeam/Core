using Cake.Frosting;
using JetBrains.Annotations;

namespace CreativeCoders.CakeBuild;

[PublicAPI]
public interface ICakeHostBuilder
{
    ICakeHostBuilder AddBuildServerIntegration();

    ICakeHostBuilder AddDefaultTasks();

    ICakeHostBuilder AddTask<TTask>()
        where TTask : class, IFrostingTask;

    ICakeHostBuilder AddTasks(params Type[] taskTypes);

    ICakeHostBuilder ConfigureHost(Action<CakeHost> configure);

    ICakeHostBuilder UseBuildContext<TBuildContext>()
        where TBuildContext : CakeBuildContext;

    ICakeHostBuilder InstallTools(params ToolInstallation[] tools);

    CakeHost Build();
}
