using Cake.Frosting;
using CreativeCoders.CakeBuild;

namespace CakeBuildSample;

internal static class Program
{
    static int Main(string[] args)
    {
        return new CakeHost()
            .SetupHost<BuildContext, BuildSetup>()
            .AddDefaultTasks()
            .AddBuildServerIntegration()
            .Run(args);
    }
}
