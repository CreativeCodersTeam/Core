using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Frosting;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using CreativeCoders.Core;
using CreativeCoders.Core.IO;
using JetBrains.Annotations;

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

    public ICakeHostBuilder InstallTools(params ToolInstallation[] tools)
    {
        var tempToolsPath = FileSys.Path.Combine(FileSys.Path.GetTempPath(), ".cake-tools");

        _cakeHost.SetToolPath(tempToolsPath);

        foreach (var tool in tools)
        {
            _cakeHost.InstallTool(new Uri($"{tool.ToolKind}:?package={tool.Name}&version={tool.Version}"));
        }

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

        _cakeHost.UseSetup<DefaultHostSetup>();

        return _cakeHost;
    }
}

[UsedImplicitly]
public class DefaultHostSetup : IFrostingSetup
{
    public void Setup(ICakeContext context, ISetupContext info)
    {
        if (context is not BuildContext buildContext)
        {
            return;
        }

        if (!buildContext.PrintSetupSummary)
        {
            return;
        }

        var pushSettings = buildContext.GetSettings<INuGetPushTaskSettings>();

        if (pushSettings != null)
        {
            context.Information($"Skip Push: {pushSettings.SkipPush}");
        }
    }
}
