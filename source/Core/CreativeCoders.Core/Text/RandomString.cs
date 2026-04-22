using System;
using System.Security.Cryptography;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core.Text;

/// <summary>
/// Provides methods for generating cryptographically random Base64-encoded strings.
/// </summary>
[PublicAPI]
public static class RandomString
{
    /// <summary>
    /// Creates a random Base64-encoded string using the specified buffer size for the random number generator.
    /// </summary>
    /// <param name="bufferSize">The number of random bytes to generate before Base64 encoding.</param>
    /// <returns>A random Base64-encoded string.</returns>
    public static string Create(int bufferSize)
    {
        var buffer = new byte[bufferSize];

        RandomNumberGenerator.Create().GetNonZeroBytes(buffer);

        return Convert.ToBase64String(buffer);
    }

    /// <summary>
    /// Creates a random Base64-encoded string using a default buffer size of 128 bytes.
    /// </summary>
    /// <returns>A random Base64-encoded string.</returns>
    /// <seealso cref="Create(int)"/>
    public static string Create()
    {
        return Create(128);
    }
}
