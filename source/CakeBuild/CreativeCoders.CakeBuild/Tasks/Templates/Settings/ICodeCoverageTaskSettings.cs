using Cake.Common.Tools.ReportGenerator;

namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

[CakeTaskSettings]
public interface ICodeCoverageTaskSettings : IBuildContextAccessor
{
    IEnumerable<ReportGeneratorReportType> ReportTypes =>
    [
        ReportGeneratorReportType.Html,
        ReportGeneratorReportType.MarkdownSummaryGithub
    ];

    string ReportGlobPattern => Context.CodeCoverageDir.FullPath + "/**/*.xml";
}
