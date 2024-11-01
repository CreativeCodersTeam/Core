using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core;

[PublicAPI]
[ExcludeFromCodeCoverage]
public static class NullObject
{
    public static object Instance { get; } = new object();
}
