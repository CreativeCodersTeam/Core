using System.Reflection;
using Cake.Common.Diagnostics;
using Cake.Frosting;
using JetBrains.Annotations;

namespace CreativeCoders.CakeBuild.Tasks;

[PublicAPI]
public abstract class FrostingTaskBase<T> : AsyncFrostingTask<T>
    where T : CakeBuildContext
{
    public sealed override async Task RunAsync(T context)
    {
        var taskName = ReadTaskName();

        context.Information($"Running task '{taskName}' ...");

        await RunAsyncCore(context).ConfigureAwait(false);
    }

    protected string ReadTaskName()
    {
        var taskNameAttr = GetType().GetCustomAttribute<TaskNameAttribute>();

        return taskNameAttr?.Name ?? GetType().Name;
    }

    protected abstract Task RunAsyncCore(T context);

    public override void Finally(T context)
    {
        base.Finally(context);

        context.AddExecutedTask(this);
    }
}

public abstract class FrostingTaskBase<TBuildContext, TSettings> : FrostingTaskBase<TBuildContext>
    where TBuildContext : CakeBuildContext
    where TSettings : class
{
    protected sealed override Task RunAsyncCore(TBuildContext context)
    {
        var taskSettings = context.GetRequiredSettings<TSettings>();

        return RunAsyncCore(context, taskSettings);
    }

    protected abstract Task RunAsyncCore(TBuildContext context, TSettings taskSettings);
}
