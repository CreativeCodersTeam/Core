using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Core;
using CreativeCoders.NukeBuild.Components.Parameters;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.Components.Targets;

[PublicAPI]
public interface ICleanTarget : INukeBuild, ICleanSettings
{
    Target Clean => d => d
        .TryBefore<IRestoreTarget>()
        .Executes(() =>
        {
            DotNetTasks.DotNetClean(x => x
                .Apply(ConfigureCleanSettings));

            DirectoriesToClean.SafeDeleteDirectories();
        });

    DotNetCleanSettings ConfigureCleanSettings(DotNetCleanSettings cleanSettings)
        => ConfigureDefaultCleanSettings(cleanSettings);

    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    sealed DotNetCleanSettings ConfigureDefaultCleanSettings(DotNetCleanSettings cleanSettings)
    {
        var solutionParameter = this.As<ISolutionParameter>();

        return cleanSettings
            .When(_ => solutionParameter != null,
                x => x.SetProject(solutionParameter!.Solution));
    }
}
