using Cake.Common.Diagnostics;
using Cake.Common.Tools.ReportGenerator;
using Cake.Core.IO;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

public class CodeCoverageTask<T> : FrostingTaskBase<T>
    where T : BuildContext
{
    protected override Task RunAsyncCore(T context)
    {
        var reportGeneratorSettings = new ReportGeneratorSettings
        {
            SetupProcessSettings = x =>
            {
                context.Information(
                    $"ReportGenerator Process Args: {string.Join("", x.Arguments.Select(arg => arg.RenderSafe()))}");
            }
        };

        context.ReportGenerator(new GlobPattern(context.CodeCoverageDir.FullPath + "/**/*.xml"),
            context.CodeCoverageReportDir, reportGeneratorSettings);

        return Task.CompletedTask;
    }
}
