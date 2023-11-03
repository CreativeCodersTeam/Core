using CreativeCoders.Core.Dependencies;

namespace CreativeCoders.Core.UnitTests.Dependencies;

public static class DependencyTestData
{
    public const string Cpu = nameof(Cpu);

    public const string Kernel = nameof(Kernel);

    public const string VgaDriver = nameof(VgaDriver);

    public const string NetworkDriver = nameof(NetworkDriver);

    public const string Shell = nameof(Shell);

    public const string Application = nameof(Application);

    public const string WebServer = nameof(WebServer);

    public const string Browser = nameof(Browser);


    public static DependencyObjectCollection<string> CreateTestData()
    {
        var collection = new DependencyObjectCollection<string>();

        collection.AddDependency(Application, Shell);

        collection.AddDependency(Shell, VgaDriver);

        collection.AddDependency(VgaDriver, Kernel);

        collection.AddDependency(Kernel, Cpu);

        return collection;
    }

    public static DependencyObjectCollection<string> CreateTestDataForTree()
    {
        var collection = new DependencyObjectCollection<string>();

        collection.AddDependency(Application, Shell);

        collection.AddDependency(Shell, VgaDriver);

        collection.AddDependency(VgaDriver, Kernel);

        collection.AddDependency(NetworkDriver, Kernel);

        collection.AddDependency(WebServer, NetworkDriver);

        collection.AddDependency(Browser, Application, NetworkDriver);

        return collection;
    }

    public static DependencyObjectCollection<string> CreateTestDataWithCircularReference()
    {
        var collection = new DependencyObjectCollection<string>();

        collection.AddDependency(Application, Shell);

        collection.AddDependency(Shell, VgaDriver);

        collection.AddDependency(VgaDriver, Kernel);

        collection.AddDependency(Kernel, Cpu);

        collection.AddDependency(Shell, Application);

        return collection;
    }
}
