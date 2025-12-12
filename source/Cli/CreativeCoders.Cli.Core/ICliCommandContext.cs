using JetBrains.Annotations;

namespace CreativeCoders.Cli.Core;

[PublicAPI]
public interface ICliCommandContext
{
    public string[] AllArgs { get; set; }

    public string[] OptionsArgs { get; set; }
}
