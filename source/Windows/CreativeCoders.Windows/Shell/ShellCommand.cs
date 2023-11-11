using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CreativeCoders.Windows.Shell;

[PublicAPI]
[ExcludeFromCodeCoverage]
public class ShellCommand
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public IList<string> FileExtensions { get; } = new List<string>();

    public string Command { get; set; }

    public string Icon { get; set; }
}
