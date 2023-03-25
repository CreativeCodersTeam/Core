using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

public interface ITestTargetSettings : INukeBuild
{
    IEnumerable<Project> TestProjects { get; }

    AbsolutePath TestBaseDirectory => RootDirectory / ".tests";

    AbsolutePath TestResultsDirectory => TestBaseDirectory / "results";

    AbsolutePath CoverageDirectory => TestBaseDirectory / "coverage";
}
