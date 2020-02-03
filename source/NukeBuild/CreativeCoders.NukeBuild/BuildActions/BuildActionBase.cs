using System;
using Nuke.Common;

namespace CreativeCoders.NukeBuild.BuildActions
{
    public abstract class BuildActionBase<TBuildAction> : IBuildAction<TBuildAction>
        where TBuildAction : BuildActionBase<TBuildAction>, new()
    {
        public ITargetDefinition Setup(ITargetDefinition targetDefinition)
        {
            return Setup(targetDefinition, null);
        }

        public ITargetDefinition Setup(ITargetDefinition targetDefinition, Action<TBuildAction> configureAction)
        {
            targetDefinition.Executes(() => ExecuteAction(configureAction));

            return targetDefinition;
        }

        private void ExecuteAction(Action<TBuildAction> configureAction)
        {
            configureAction((TBuildAction) this);
            
            OnExecute();
        }

        public void SetBuildInfo(IBuildInfo buildInfo)
        {
            BuildInfo = buildInfo;
        }

        protected abstract void OnExecute();

        protected IBuildInfo BuildInfo { get; private set; }
    }
}