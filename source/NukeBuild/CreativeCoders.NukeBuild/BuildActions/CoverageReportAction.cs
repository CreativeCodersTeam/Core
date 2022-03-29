using CreativeCoders.Core.SysEnvironment;
using JetBrains.Annotations;
using Nuke.Common.Tools.ReportGenerator;

namespace CreativeCoders.NukeBuild.BuildActions;

[PublicAPI]
public class CoverageReportAction : BuildActionBase<CoverageReportAction>
{
    private string _framework;

    private string[] _reports;

    private string _targetDirectory;

    public CoverageReportAction()
    {
        _framework = $"net{Env.Version.Major}.0";
    }

    protected override void OnExecute()
    {
        ReportGeneratorTasks.ReportGenerator(x => x
            .SetFramework(_framework)
            .SetReports(_reports)
            .SetTargetDirectory(_targetDirectory));
    }

    public CoverageReportAction SetFramework(string framework)
    {
        _framework = framework;

        return this;
    }

    public CoverageReportAction SetReports(params string[] reports)
    {
        _reports = reports;

        return this;
    }

    public CoverageReportAction SetTargetDirectory(string targetDirectory)
    {
        _targetDirectory = targetDirectory;

        return this;
    }
}
