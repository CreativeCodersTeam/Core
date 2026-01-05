using CreativeCoders.CakeBuild;

namespace CakeBuildSample;

internal static class Program
{
    static int Main(string[] args)
    {
        return CakeHostBuilder.Create()
            .UseBuildContext<SampleBuildContext>()
            .AddDefaultTasks()
            .AddBuildServerIntegration()
            .Build()
            .Run(args);
    }
}
