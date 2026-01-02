using Cake.Frosting;

namespace CreativeCoders.CakeBuild.Tasks;

public class FrostingTaskBase<T> : AsyncFrostingTask<T>
    where T : BuildContext
{
    public override void Finally(T context)
    {
        base.Finally(context);

        context.AddExecutedTask(this);
    }
}
