using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core;

[PublicAPI]
[ExcludeFromCodeCoverage]
public class NullObject
{
    public static object Instance { get; } = new();
}
