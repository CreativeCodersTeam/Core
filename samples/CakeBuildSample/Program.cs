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
            .Run(args);
    }
}
