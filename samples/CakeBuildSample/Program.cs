using Cake.Core.IO;
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
            .ConfigureHost(x =>
            {
                var tempToolsPath = FileSys.Path.Combine(FileSys.Path.GetTempPath(), ".cake-tools");

                Console.WriteLine($"Using temp tools path: {tempToolsPath}");

                x.SetToolPath(tempToolsPath);

                x.InstallTool(new Uri("nuget:?package=GitVersion.Tool&version=6.5.1"));
                x.InstallTool(new Uri("nuget:?package=dotnet-reportgenerator-globaltool&version=5.5.1"));
            })
            .Build()
            .Run(args);
    }
}
