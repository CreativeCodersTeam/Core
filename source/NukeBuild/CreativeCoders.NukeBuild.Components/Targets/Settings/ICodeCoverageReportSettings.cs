using CreativeCoders.Core;
using CreativeCoders.Core.IO;
using CreativeCoders.Core.SysEnvironment;
using Nuke.Common.IO;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

public interface ICodeCoverageReportSettings : ITestSettings
{
    AbsolutePath TargetDirectory => this.TryAs<ITestSettings>(out var testSettings)
        ? testSettings.TestBaseDirectory / "coverage_report"
        : TemporaryDirectory / "coverage_report";

    AbsolutePath CoverageReportArchive => TargetDirectory.Parent / FileSys.Path.ChangeExtension(TargetDirectory.Name, "zip");

    string Framework => $"net{Env.Version.Major}.0";

    IEnumerable<string> Reports => new string[] {TestBaseDirectory / "coverage" / "**" / "*.xml"};
}
