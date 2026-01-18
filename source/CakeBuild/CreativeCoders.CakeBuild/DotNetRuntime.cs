using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CreativeCoders.CakeBuild;

[ExcludeFromCodeCoverage]
[PublicAPI]
public static class DotNetRuntime
{
    public const string WinX64 = "win-x64";

    public const string WinX86 = "win-x86";

    public const string WinArm64 = "win-arm64";

    public const string WinArm = "win-arm";

    public const string LinuxX64 = "linux-x64";

    public const string LinuxMuslX64 = "linux-x64";

    public const string LinuxArm64 = "linux-arm64";

    public const string LinuxArm = "linux-arm";

    public const string MacOsX64 = "osx-x64";
}
