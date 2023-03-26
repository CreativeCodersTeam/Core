using CreativeCoders.NukeBuild.Components.Targets.Settings;
using Nuke.Common;
using Nuke.Common.Tools.ReportGenerator;

namespace CreativeCoders.NukeBuild.Components.Targets;

public interface ICodeCoverageReportTarget : ITestTarget, ICodeCoverageReportSettings
{
    Target CodeCoverageReport => _ => _
        .DependsOn(Test)
        .Produces(TargetDirectory)
        .Executes(() =>
        {
            ReportGeneratorTasks.ReportGenerator(x => x
                .SetFramework(Framework)
                .SetReports(Reports)
                .SetTargetDirectory(TargetDirectory));
        });
}
