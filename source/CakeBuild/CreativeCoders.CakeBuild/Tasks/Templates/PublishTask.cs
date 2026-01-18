using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Publish;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using JetBrains.Annotations;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

[PublicAPI]
public class PublishTask<TBuildContext> : FrostingTaskBase<TBuildContext, IPublishTaskSettings>
    where TBuildContext : CakeBuildContext
{
    protected override Task RunAsyncCore(TBuildContext context, IPublishTaskSettings taskSettings)
    {
        if (taskSettings.PublishingItems.Any())
        {
            foreach (var publishingItem in taskSettings.PublishingItems)
            {
                context.DotNetPublish(publishingItem.ProjectPath.FullPath,
                    CreateDotNetPublishSettings(taskSettings, publishingItem, context));
            }
        }
        else
        {
            context.DotNetPublish(context.SolutionFile.FullPath,
                CreateDotNetPublishSettings(taskSettings, context));
        }

        return Task.CompletedTask;
    }

    private static DotNetPublishSettings CreateDotNetPublishSettings(IPublishTaskSettings publishSettings,
        TBuildContext context)
    {
        var dotNetPublishSettings = new DotNetPublishSettings
        {
            OutputDirectory = publishSettings.PublishOutputDir,
            NoBuild = context.HasExecutedTask(typeof(BuildTask<TBuildContext>)),
            Configuration = context.BuildConfiguration
        };

        return dotNetPublishSettings;
    }

    protected virtual DotNetPublishSettings CreateDotNetPublishSettings(IPublishTaskSettings publishSettings,
        PublishingItem publishingItem, TBuildContext context)
    {
        var dotNetPublishSettings = new DotNetPublishSettings
        {
            Runtime = publishingItem.Runtime,
            OutputDirectory = publishingItem.OutputDir.IsRelative
                ? publishSettings.PublishOutputDir.Combine(publishingItem.OutputDir)
                : publishingItem.OutputDir,
            SelfContained = publishingItem.SelfContained,
            NoBuild = context.HasExecutedTask(typeof(BuildTask<TBuildContext>)),
            Configuration = context.BuildConfiguration
        };

        return dotNetPublishSettings;
    }

    protected override bool SkipIfNoSettings => true;
}
