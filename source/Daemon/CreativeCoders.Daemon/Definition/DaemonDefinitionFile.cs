using System.Text.Json;
using CreativeCoders.Core.IO;

namespace CreativeCoders.Daemon.Definition;

public static class DaemonDefinitionFile
{
    public static async Task<DaemonDefinition?> LoadAsync(string fileName)
    {
        return await JsonSerializer
            .DeserializeAsync<DaemonDefinition>(FileSys.File.OpenRead(fileName))
            .ConfigureAwait(false);
    }
}
