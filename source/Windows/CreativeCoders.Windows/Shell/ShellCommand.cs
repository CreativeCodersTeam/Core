using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Windows.Shell;

[PublicAPI]
public class ShellCommand
{
    public ShellCommand()
    {
        FileExtensions = new List<string>();
    }

    public string Name { get; set; }

    public string DisplayName { get; set; }

    public IList<string> FileExtensions { get; }

    public string Command { get; set; }

    public string Icon { get; set; }
}