using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.BuildActions;

public class DotNetPublishBuildAction : BuildActionBase<DotNetPublishBuildAction>
{
    private string _projectFile;

    private string _output;

    private string _runtime;

    private bool _selfContained;

    protected override void OnExecute()
    {
        DotNetTasks.DotNetPublish(x =>
        {
            var settings = x
                .SetProject(_projectFile)
                .SetConfiguration(BuildInfo.Configuration)
                .SetOutput(_output)
                .SetVersion(BuildInfo.VersionInfo.NuGetVersionV2)
                .SetSelfContained(_selfContained);

            if (!string.IsNullOrWhiteSpace(_runtime))
            {
                settings = settings.SetRuntime(_runtime);
            }

            return settings;
        });
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

    public DotNetPublishBuildAction SetRuntime(string runtime)
    {
        _runtime = runtime;

        return this;
    }

    public DotNetPublishBuildAction SetSelfContained(bool selfContained)
    {
        _selfContained = selfContained;

        return this;
    }
}
