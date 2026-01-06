using Cake.Common.Tools.ReportGenerator;
using Cake.Core.IO;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

public class CodeCoverageTask<T> : FrostingTaskBase<T>
    where T : BuildContext
{
    protected override Task RunAsyncCore(T context)
    {
        context.ReportGenerator(new GlobPattern(context.CodeCoverageDir.FullPath + "/**/*.xml"),
            context.CodeCoverageReportDir);

        return Task.CompletedTask;
    }
}
