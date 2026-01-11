using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Publish;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

public class PublishTask<TBuildContext> : FrostingTaskBase<TBuildContext>
    where TBuildContext : CakeBuildContext
{
    protected override Task RunAsyncCore(TBuildContext context)
    {
        var publishSettings = context.GetRequiredSettings<IPublishTaskSettings>();

        if (publishSettings.PublishingItems.Any())
        {
            foreach (var publishingItem in publishSettings.PublishingItems)
            {
                context.DotNetPublish(publishingItem.ProjectPath.FullPath,
                    CreateDotNetPublishSettings(publishingItem, context));
            }
        }
        else
        {
            context.DotNetPublish(context.SolutionFile.FullPath,
                CreateDotNetPublishSettings(publishSettings, context));
        }

        return Task.CompletedTask;
    }

    private DotNetPublishSettings CreateDotNetPublishSettings(IPublishTaskSettings publishSettings,
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

    protected virtual DotNetPublishSettings CreateDotNetPublishSettings(PublishingItem publishingItem,
        TBuildContext context)
    {
        var dotNetPublishSettings = new DotNetPublishSettings
        {
            Runtime = publishingItem.Runtime,
            OutputDirectory = publishingItem.OutputDir,
            SelfContained = publishingItem.SelfContained,
            NoBuild = context.HasExecutedTask(typeof(BuildTask<TBuildContext>)),
            Configuration = context.BuildConfiguration
        };

        return dotNetPublishSettings;
    }
}
