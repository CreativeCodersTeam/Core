using System.Diagnostics.CodeAnalysis;

namespace WindowsServiceDaemonSampleApp;

public class SampleWorkerConfig
{
    [SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
    public string TestData { get; set; }
}
