using Cake.Frosting;
using CreativeCoders.Core;

namespace CreativeCoders.CakeBuild;

public class CakeHostBuilder : ICakeHostBuilder
{
    private readonly CakeHost _cakeHost = new CakeHost();

    private bool _buildContextIsSet;

    public static ICakeHostBuilder Create()
    {
        return new CakeHostBuilder();
    }

    public ICakeHostBuilder UseBuildContext<TBuildContext>()
        where TBuildContext : BuildContext
    {
        _buildContextIsSet = true;

        _cakeHost.UseContext<TBuildContext>();

        return this;
    }

    public ICakeHostBuilder UseBuildSetup<TBuildSetup>()
        where TBuildSetup : class
    {
        _cakeHost.UseBuildSetup<TBuildSetup>();

        return this;
    }

    public ICakeHostBuilder AddBuildServerIntegration()
    {
        _cakeHost.AddBuildServerIntegration();

        return this;
    }

    public ICakeHostBuilder AddDefaultTasks()
    {
        _cakeHost.AddDefaultTasks();

        return this;
    }

    public ICakeHostBuilder AddTask<TTask>()
        where TTask : class, IFrostingTask
    {
        _cakeHost.AddTask<TTask>();

        return this;
    }

    public ICakeHostBuilder AddTasks(params Type[] taskTypes)
    {
        _cakeHost.AddTasks(taskTypes);

        return this;
    }

    public ICakeHostBuilder ConfigureHost(Action<CakeHost> configure)
    {
        Ensure.NotNull(configure);

        configure(_cakeHost);

        return this;
    }

    public CakeHost Build()
    {
        if (!_buildContextIsSet)
        {
            _cakeHost.UseContext<BuildContext>();
        }

        return _cakeHost;
    }
}
