using Cake.Frosting;

namespace CakeBuildSample;

internal static class Program
{
    static int Main(string[] args)
    {
        return new CakeHost()
            .UseTaskSetup<>()
            .Run(args);
    }
}
