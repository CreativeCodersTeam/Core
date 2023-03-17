using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using CreativeCoders.Core.IO;

namespace CreativeCoders.Daemon.Definition;

public static class DaemonDefinitionFile
{
    /// <summary>
    /// Load daemon definition from the specified file
    /// </summary>
    /// <param name="fileName">File to read daemon definition from</param>
    /// <returns></returns>
    public static async Task<DaemonDefinition?> LoadAsync(string fileName)
    {
        return await JsonSerializer
            .DeserializeAsync<DaemonDefinition>(
                FileSys.File.OpenRead(fileName),
                new JsonSerializerOptions{DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull})
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Save daemon definition to the specified file
    /// </summary>
    /// <param name="daemonDefinition">The daemon definition which gets saved to file</param>
    /// <param name="fileName">The file to which the daemon definition is written</param>
    public static async Task Save(this DaemonDefinition daemonDefinition, string fileName)
    {
        await JsonSerializer
            .SerializeAsync(FileSys.File.Create(fileName), daemonDefinition)
            .ConfigureAwait(false);
    }
}
