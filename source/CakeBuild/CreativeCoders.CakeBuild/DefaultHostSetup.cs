using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Frosting;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using JetBrains.Annotations;

namespace CreativeCoders.CakeBuild;

[UsedImplicitly]
public class DefaultHostSetup : IFrostingSetup
{
    public void Setup(ICakeContext context, ISetupContext info)
    {
        if (context is not CakeBuildContext buildContext)
        {
            return;
        }

        if (!buildContext.PrintSetupSummary)
        {
            return;
        }

        context.Information("Build Setup Summary");
        context.Information("===================");
        context.Information("Tasks for execution:");

        foreach (var task in info.TasksToExecute)
        {
            context.Information($"- {task.Name}");
        }

        var pushSettings = buildContext.GetSettings<INuGetPushTaskSettings>();

        if (pushSettings != null)
        {
            context.Information($"Skip Push: {pushSettings.SkipPush}");
        }

        var version = buildContext.Version;

        context.Information("Version Info");
        context.Information("============");

        context.Information($"Informational Version: {version.InformationalVersion}");
        context.Information($"Assembly Version: {version.AssemblySemVer}");
        context.Information($"File Version: {version.AssemblySemFileVer}");
        context.Information($"Package Version: {version.SemVer}");
    }
}
