#nullable enable
namespace CreativeCoders.Core.Text;

/// <summary>
/// Represents an immutable key-value pair of strings.
/// </summary>
/// <param name="key">The key.</param>
/// <param name="value">The value.</param>
public class KeyAndValue(string key, string value)
{
    /// <summary>
    /// Gets the key.
    /// </summary>
    public string Key { get; } = Ensure.NotNull(key);

    /// <summary>
    /// Gets the value.
    /// </summary>
    public string Value { get; } = Ensure.NotNull(value);
}
