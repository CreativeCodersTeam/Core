using CreativeCoders.CakeBuild;

namespace CakeBuildSample;

internal static class Program
{
    static int Main(string[] args)
    {
        return CakeHostBuilder.Create()
            .UseBuildSetup<BuildSetup>()
            .AddDefaultTasks()
            .AddBuildServerIntegration()
            .Build()
            .Run(args);
    }
}
