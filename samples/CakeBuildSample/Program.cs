using CreativeCoders.CakeBuild;
using CreativeCoders.CakeBuild.Tasks.Defaults;

namespace CakeBuildSample;

internal static class Program
{
    static int Main(string[] args)
    {
        return CakeHostBuilder.Create()
            .UseBuildContext<SampleBuildContext>()
            .AddDefaultTasks()
            .Build()
            .Run(args);
    }
}
