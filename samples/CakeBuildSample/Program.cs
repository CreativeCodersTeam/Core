using Cake.Core.IO;
using Cake.DotNetTool.Module;
using Cake.Frosting;
using CreativeCoders.CakeBuild;
using CreativeCoders.Core.IO;
using CreativeCoders.Core.SysEnvironment;

namespace CakeBuildSample;

internal static class Program
{
    static int Main(string[] args)
    {
        return CakeHostBuilder.Create()
            .UseBuildContext<SampleBuildContext>()
            .AddDefaultTasks()
            .AddBuildServerIntegration()
            .InstallTools(
                new DotNetToolInstallation("GitVersion.Tool", "6.5.1"),
                new DotNetToolInstallation("dotnet-reportgenerator-globaltool", "5.5.1"))
            .Build()
            .Run(args);
    }
}
