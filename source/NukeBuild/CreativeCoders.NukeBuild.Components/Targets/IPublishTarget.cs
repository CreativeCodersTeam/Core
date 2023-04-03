using System.Diagnostics.CodeAnalysis;
using CreativeCoders.NukeBuild.Components.Parameters;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.Components.Targets;

[PublicAPI]
public interface IPublishTarget : IPublishSettings
{
    Target Publish => _ => _
        .TryAfter<ICodeCoverageReportTarget>()
        .Produces(PublishingItems.Any()
            ? PublishingItems.Select(x => x.OutputPath.ToString()).ToArray()
            : new string[] {PublishOutputPath})
        .Executes(() =>
        {
            if (PublishingItems.Any())
            {
                DotNetTasks.DotNetPublish(x => x
                    .Apply(ConfigurePublishSettings)
                    .CombineWith(PublishingItems, ConfigurePublishItemSettings));
            }
            else
            {
                DotNetTasks.DotNetPublish(x => x
                    .Apply(ConfigurePublishSettings));
            }
        });

    DotNetPublishSettings ConfigurePublishSettings(DotNetPublishSettings publishSettings)
        => ConfigureDefaultPublishSettings(publishSettings);

    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    sealed DotNetPublishSettings ConfigureDefaultPublishSettings(DotNetPublishSettings publishSettings)
    {
        return publishSettings
            .WhenNotNull(this as ISolutionParameter, (x, solutionParameter) => x
                .SetProject(solutionParameter.Solution))
            .SetOutput(PublishOutputPath)
            .WhenNotNull(this as IConfigurationParameter, (x, configuration) => x
                .SetConfiguration(configuration.Configuration))
            .WhenNotNull((this as IGitVersionParameter)?.GitVersion, (x, gitVersion) => x
                .SetAssemblyVersion(gitVersion.AssemblySemVer)
                .SetFileVersion(gitVersion.AssemblySemFileVer)
                .SetInformationalVersion(gitVersion.InformationalVersion)
                .SetVersion(gitVersion.FullSemVer));
    }

    DotNetPublishSettings ConfigurePublishItemSettings(DotNetPublishSettings publishSettings,
        PublishingItem publishingItem)
        => ConfigureDefaultPublishItemSettings(publishSettings, publishingItem);

    sealed DotNetPublishSettings ConfigureDefaultPublishItemSettings(DotNetPublishSettings publishSettings,
        PublishingItem publishingItem)
    {
        return publishSettings
            .SetProject(publishingItem.ProjectPath)
            .SetOutput(publishingItem.OutputPath)
            .SetSelfContained(publishingItem.SelfContained)
            .When(!string.IsNullOrWhiteSpace(publishingItem.Runtime), x => x
                .SetRuntime(publishingItem.Runtime));
    }
}
