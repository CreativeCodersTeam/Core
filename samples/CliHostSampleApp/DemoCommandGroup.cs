using CreativeCoders.Cli.Core;

[assembly: CliCommandGroup(["demo"], "Demo commands root group")]

namespace CliHostSampleApp;

public static class DemoCommandGroup
{
    public const string Name = "demo";
}
