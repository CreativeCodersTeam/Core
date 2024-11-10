using CreativeCoders.Core;
using CreativeCoders.Core.IO;
using CreativeCoders.Core.SysEnvironment;
using CreativeCoders.Options.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Options.Storage.FileSystem;

[PublicAPI]
public class FileSystemOptionsStorageProvider<T>(IOptionsStorageDataSerializer optionsSerializer)
    : IOptionsStorageProvider<T>
    where T : class
{
    private readonly IOptionsStorageDataSerializer _optionsSerializer = Ensure.NotNull(optionsSerializer);

    private string GetFileName(string? name)
    {
        name = string.IsNullOrWhiteSpace(name)
            ? DefaultFileName
            : FileSys.Path.ReplaceInvalidFileNameChars(name, InvalidCharReplacement);

        var fileName = FileSys.Path.Combine(DirectoryPath, ConvertNameToFileName(name ?? DefaultFileName));

        return fileName;
    }

    public async Task WriteAsync(string? name, T options)
    {
        var fileName = GetFileName(name);

        await FileSys.File.WriteAllTextAsync(fileName, _optionsSerializer.Serialize(options))
            .ConfigureAwait(false);
    }

    public void Write(string? name, T options)
    {
        var fileName = GetFileName(name);

        FileSys.File.WriteAllText(fileName, _optionsSerializer.Serialize(options));
    }

    public async Task ReadAsync(string? name, T options)
    {
        var fileName = GetFileName(name);

        _optionsSerializer.Deserialize(await FileSys.File.ReadAllTextAsync(fileName).ConfigureAwait(false),
            options);
    }

    public void Read(string? name, T options)
    {
        var fileName = GetFileName(name);

        _optionsSerializer.Deserialize(FileSys.File.ReadAllText(fileName), options);
    }

    public string DirectoryPath { get; set; } = Env.CurrentDirectory;

    public string DefaultFileName { get; set; } = "default";

    public Func<string, string> ConvertNameToFileName { get; set; } = name => $"{name}.options";

    public string InvalidCharReplacement { get; set; } = "_";
}
