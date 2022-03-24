using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.BuildActions
{
    public class DotNetPublishBuildAction : BuildActionBase<DotNetPublishBuildAction>
    {
        private string _projectFile;

        private string _output;

        protected override void OnExecute()
        {
            DotNetTasks.DotNetPublish(x => x
                .SetProject(_projectFile)
                .SetConfiguration(BuildInfo.Configuration)
                .SetOutput(_output)
                .SetVersion(BuildInfo.VersionInfo.NuGetVersionV2));
        }

        public DotNetPublishBuildAction SetProject(string projectFile)
        {
            _projectFile = projectFile;

            return this;
        }

        public DotNetPublishBuildAction SetOutput(string output)
        {
            _output = output;

            return this;
        }
    }
}
