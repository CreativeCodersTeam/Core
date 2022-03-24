using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.BuildActions
{
    public class DotNetPublishBuildAction : BuildActionBase<DotNetPublishBuildAction>
    {
        private string _projectFile;

        protected override void OnExecute()
        {
            DotNetTasks.DotNetPublish(x => x
                .SetProject(_projectFile)
                .SetConfiguration(BuildInfo.Configuration)
                .SetOutput(BuildInfo.ArtifactsDirectory)
                .SetVersion(BuildInfo.VersionInfo.NuGetVersionV2));
        }

        public DotNetPublishBuildAction AddProject(string projectFile)
        {
            _projectFile = projectFile;

            return this;
        }
    }
}
