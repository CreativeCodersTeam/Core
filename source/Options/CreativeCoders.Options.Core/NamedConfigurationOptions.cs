using CreativeCoders.Core;
using Microsoft.Extensions.Options;

namespace CreativeCoders.Options.Core;

public class NamedConfigurationOptions<T> : IConfigureNamedOptions<T> where T : class
{
    private readonly IOptionsStorageProvider<T> _optionsReader;

    public NamedConfigurationOptions(IOptionsStorageProvider<T> optionsReader)
    {
        _optionsReader = Ensure.NotNull(optionsReader);
    }

    public void Configure(T options)
    {
        _optionsReader.Read(string.Empty, options);
    }

    public void Configure(string? name, T options)
    {
        _optionsReader.Read(name, options);
    }
}
