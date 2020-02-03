using System;

namespace CreativeCoders.Di.Exceptions
{
    public class ResolveFailedException : Exception
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public ResolveFailedException(Type serviceType, Exception innerException) : base(
            $"Resolving service '{serviceType.Name}' failed", innerException) { }
    }
}