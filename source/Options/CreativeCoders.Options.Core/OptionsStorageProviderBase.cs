using CreativeCoders.Core;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace CreativeCoders.Options.Core;

[PublicAPI]
public abstract class OptionsStorageProviderBase<T> : IOptionsStorageProvider<T>
    where T : class
{
    protected readonly IOptionsMonitorCache<T> OptionsCache;

    protected OptionsStorageProviderBase(IOptionsMonitorCache<T> optionsCache)
    {
        OptionsCache = Ensure.NotNull(optionsCache);
    }

    protected abstract Task InternalWriteAsync(string? name, T options);

    protected abstract void InternalWrite(string? name, T options);

    public Task WriteAsync(string? name, T options)
    {
        OptionsCache.TryRemove(name);

        return InternalWriteAsync(name, options);
    }

    public void Write(string? name, T options)
    {
        OptionsCache.TryRemove(name);

        InternalWrite(name, options);
    }

    public abstract Task ReadAsync(string? name, T options);

    public abstract void Read(string? name, T options);
}
