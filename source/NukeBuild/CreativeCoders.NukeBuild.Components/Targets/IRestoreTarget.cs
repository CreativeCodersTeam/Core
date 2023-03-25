﻿using CreativeCoders.Core;
using CreativeCoders.NukeBuild.Components.Parameters;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.Components.Targets;

public interface IRestoreTarget : INukeBuild
{
    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetTasks.DotNetRestore(x => x
                .Apply(ConfigureRestoreSettings));
        });

    DotNetRestoreSettings ConfigureRestoreSettings(DotNetRestoreSettings buildSettings)
        => ConfigureDefaultRestoreSettings(buildSettings);

    sealed DotNetRestoreSettings ConfigureDefaultRestoreSettings(DotNetRestoreSettings buildSettings)
    {
        return buildSettings
            .When(this.TryAs<ISolutionParameter>(out var solutionParameter), x => x
                .SetProjectFile(solutionParameter!.Solution));
    }
}
