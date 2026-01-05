using Cake.Common.Tools.DotNet;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

public class RestoreTask<T> : FrostingTaskBase<T>
    where T : BuildContext
{
    protected override Task RunAsyncCore(T context)
    {
        context.DotNetRestore(context.RootDir.FullPath);

        return Task.CompletedTask;
    }
}
