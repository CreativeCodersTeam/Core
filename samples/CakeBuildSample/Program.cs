using Cake.Common.Build;
using Cake.Core;
using Cake.Frosting;
using CreativeCoders.CakeBuild;
using CreativeCoders.CakeBuild.Tasks.Defaults;

namespace CakeBuildSample;

internal static class Program
{
    static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .UseBuildSetup<BuildSetup>()
            .AddTask<CleanTask>()
            .AddTask<RestoreTask>()
            .AddTask<BuildTask>()
            .UseTaskSetup<SomeTaskSetup>()
            .UseTaskTeardown<SomeTaskTeardown>()
            .Run(args);
    }
}

public class SomeTaskTeardown : IFrostingTaskTeardown
{
    void IFrostingTaskTeardown.Teardown(ICakeContext context, ITaskTeardownContext info)
    {
        context.GitHubActions().Commands.EndGroup();
    }
}

public class SomeTaskSetup : IFrostingTaskSetup
{
    void IFrostingTaskSetup.Setup(ICakeContext context, ITaskSetupContext info)
    {
        context.GitHubActions().Commands.StartGroup(info.Task.Name);
    }
}
