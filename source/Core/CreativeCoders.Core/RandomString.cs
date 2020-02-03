using System;
using System.Security.Cryptography;
using JetBrains.Annotations;

namespace CreativeCoders.Core
{
    [PublicAPI]
    public static class RandomString
    {
        public static string New(int bufferSize)
        {
            using var rng = new RNGCryptoServiceProvider();
            
            var buffer = new byte[bufferSize];

            rng.GetNonZeroBytes(buffer);

            return Convert.ToBase64String(buffer);
        }

        public static string New()
        {
            return New(128);
        }
    }
}
