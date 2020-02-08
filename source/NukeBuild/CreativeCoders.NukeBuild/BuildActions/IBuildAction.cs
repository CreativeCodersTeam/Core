using System;
using Nuke.Common;

namespace CreativeCoders.NukeBuild.BuildActions
{
    public interface IBuildAction<out TBuildAction>
        where TBuildAction : IBuildAction<TBuildAction>
    {
        ITargetDefinition Setup(ITargetDefinition targetDefinition);

        ITargetDefinition Setup(ITargetDefinition targetDefinition, Action<TBuildAction> configureAction);

        void SetBuildInfo(IBuildInfo buildInfo);
    }
}