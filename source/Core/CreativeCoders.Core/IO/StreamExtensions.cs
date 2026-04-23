using System.IO;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace CreativeCoders.Core.IO;

/// <summary>
/// Provides extension methods for reading <see cref="Stream"/> content as strings.
/// </summary>
public static class StreamExtensions
{
    /// <summary>
    /// Reads the entire stream content as a string using the default encoding.
    /// </summary>
    /// <param name="stream">The stream to read.</param>
    /// <returns>The string content of the stream.</returns>
    public static string ReadAsString(this Stream stream)
    {
        using var streamReader = new StreamReader(stream);
        return streamReader.ReadToEnd();
    }

    /// <summary>
    /// Reads the entire stream content as a string using the specified encoding.
    /// </summary>
    /// <param name="stream">The stream to read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <returns>The string content of the stream.</returns>
    public static string ReadAsString(this Stream stream, Encoding encoding)
    {
        using var streamReader = new StreamReader(stream, encoding);
        return streamReader.ReadToEnd();
    }

    /// <summary>
    /// Reads the entire stream content as a string using the specified encoding and byte order mark detection.
    /// </summary>
    /// <param name="stream">The stream to read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">
    /// <see langword="true"/> to detect encoding from the byte order marks; otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>The string content of the stream.</returns>
    public static string ReadAsString(this Stream stream, Encoding encoding,
        bool detectEncodingFromByteOrderMarks)
    {
        using var streamReader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks);
        return streamReader.ReadToEnd();
    }

    /// <summary>
    /// Asynchronously reads the entire stream content as a string using the default encoding.
    /// </summary>
    /// <param name="stream">The stream to read.</param>
    /// <returns>A task containing the string content of the stream.</returns>
    public static async Task<string> ReadAsStringAsync(this Stream stream)
    {
        using var streamReader = new StreamReader(stream);
        return await streamReader.ReadToEndAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously reads the entire stream content as a string using the specified encoding.
    /// </summary>
    /// <param name="stream">The stream to read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <returns>A task containing the string content of the stream.</returns>
    public static async Task<string> ReadAsStringAsync(this Stream stream, Encoding encoding)
    {
        using var streamReader = new StreamReader(stream, encoding);
        return await streamReader.ReadToEndAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously reads the entire stream content as a string using the specified encoding and byte order mark detection.
    /// </summary>
    /// <param name="stream">The stream to read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">
    /// <see langword="true"/> to detect encoding from the byte order marks; otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>A task containing the string content of the stream.</returns>
    public static async Task<string> ReadAsStringAsync(this Stream stream, Encoding encoding,
        bool detectEncodingFromByteOrderMarks)
    {
        using var streamReader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks);
        return await streamReader.ReadToEndAsync().ConfigureAwait(false);
    }
}
