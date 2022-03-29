using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Weak;

[PublicAPI]
[ExcludeFromCodeCoverage]
public class NullDisposable : IDisposable
{
    public void Dispose() { }
}
