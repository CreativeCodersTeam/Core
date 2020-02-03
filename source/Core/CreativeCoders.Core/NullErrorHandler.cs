using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CreativeCoders.Core
{
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class NullErrorHandler : IErrorHandler
    {
        public static IErrorHandler Instance { get; } = new NullErrorHandler();
        
        public void HandleException(Exception exception)
        {
        }
    }
}