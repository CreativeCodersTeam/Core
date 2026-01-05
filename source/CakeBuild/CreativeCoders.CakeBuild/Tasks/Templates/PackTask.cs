namespace CreativeCoders.CakeBuild.Tasks.Templates;

public class PackTask<T> : FrostingTaskBase<T>
    where T : BuildContext
{
    protected override Task RunAsyncCore(T context)
    {
        throw new NotImplementedException();
    }
}
