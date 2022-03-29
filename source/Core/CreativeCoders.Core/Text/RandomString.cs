using System;
using System.Security.Cryptography;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Text;

/// <summary>   A static class for creating a random string. </summary>
[PublicAPI]
public static class RandomString
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Creates a random base64 string. </summary>
    ///
    /// <param name="bufferSize">   Size of the buffer used for the random number generator. </param>
    ///
    /// <returns>   The random base64 string. </returns>
    ///-------------------------------------------------------------------------------------------------
    public static string Create(int bufferSize)
    {
        using var rng = new RNGCryptoServiceProvider();
            
        var buffer = new byte[bufferSize];

        rng.GetNonZeroBytes(buffer);

        return Convert.ToBase64String(buffer);
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Creates a random base64 string with a bufferSize of 128. <seealso cref="Create(int)"/> </summary>
    ///
    /// <returns>   The random base64 string. </returns>
    ///-------------------------------------------------------------------------------------------------
    public static string Create()
    {
        return Create(128);
    }
}