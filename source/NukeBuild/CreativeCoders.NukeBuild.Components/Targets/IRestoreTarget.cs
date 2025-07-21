using CreativeCoders.Core;
using CreativeCoders.NukeBuild.Components.Parameters;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.Components.Targets;

[PublicAPI]
public interface IRestoreTarget : INukeBuild
{
    Target Restore => d => d
        .TryBefore<IBuildTarget>()
        .Executes(() =>
        {
            DotNetTasks.DotNetRestore(x => x
                .Apply(ConfigureRestoreSettings));
        });

    DotNetRestoreSettings ConfigureRestoreSettings(DotNetRestoreSettings buildSettings)
        => ConfigureDefaultRestoreSettings(buildSettings);

    sealed DotNetRestoreSettings ConfigureDefaultRestoreSettings(DotNetRestoreSettings buildSettings)
    {
        var solutionParameter = this.As<ISolutionParameter>();

        return buildSettings
            .When(_ => solutionParameter != null, x => x
                // ReSharper disable once NullableWarningSuppressionIsUsed
                .SetProjectFile(solutionParameter!.Solution));
    }
}
