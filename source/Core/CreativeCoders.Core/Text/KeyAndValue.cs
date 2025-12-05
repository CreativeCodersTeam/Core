#nullable enable
namespace CreativeCoders.Core.Text;

public class KeyAndValue(string key, string value)
{
    public string Key { get; } = Ensure.NotNull(key);

    public string Value { get; } = Ensure.NotNull(value);
}
