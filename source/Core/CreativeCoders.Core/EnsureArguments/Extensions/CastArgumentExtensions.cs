using System.Collections.Generic;

#nullable enable
// ReSharper disable once CheckNamespace
namespace CreativeCoders.Core
{
    public static class CastArgumentExtensions
    {
        public static TValue? As<T, TValue>(in this Argument<T> argument)
            where T : TValue
        {
            return argument.Value;
        }
        
        public static TValue As<T, TValue>(in this ArgumentNotNull<T> argument)
            where T : TValue
        {
            return argument.Value;
        }
    }
}
