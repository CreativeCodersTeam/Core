using System;
using CreativeCoders.NukeBuild.BuildActions;
using JetBrains.Annotations;
using Nuke.Common;

namespace CreativeCoders.NukeBuild;

[PublicAPI]
public static class TargetDefinitionExtensions
{
    public static ITargetDefinition UseBuildAction<TBuildAction>(this ITargetDefinition targetDefinition)
        where TBuildAction : IBuildAction<TBuildAction>, new()
    {
        var action = new TBuildAction();

        return action.Setup(targetDefinition);
    }

    public static ITargetDefinition UseBuildAction<TBuildAction>(this ITargetDefinition targetDefinition,
        Action<TBuildAction> configureAction)
        where TBuildAction : IBuildAction<TBuildAction>, new()
    {
        var action = new TBuildAction();

        return action.Setup(targetDefinition, configureAction);
    }

    public static ITargetDefinition UseBuildAction<TBuildAction>(this ITargetDefinition targetDefinition,
        IBuildInfo buildInfo)
        where TBuildAction : IBuildAction<TBuildAction>, new()
    {
        var action = new TBuildAction();

        return action.Setup(targetDefinition, x => x.SetBuildInfo(buildInfo));
    }

    public static ITargetDefinition UseBuildAction<TBuildAction>(this ITargetDefinition targetDefinition,
        IBuildInfo buildInfo, Action<TBuildAction> configureAction)
        where TBuildAction : IBuildAction<TBuildAction>, new()
    {
        var action = new TBuildAction();

        return action.Setup(targetDefinition,
            x =>
            {
                x.SetBuildInfo(buildInfo);

                configureAction(x);
            });
    }

    public static ITargetDefinition ExecuteTarget(this ITargetDefinition targetDefinition, Target target)
    {
        return targetDefinition.Executes(() => target.Invoke(targetDefinition));
    }
}
