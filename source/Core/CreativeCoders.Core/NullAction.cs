#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CreativeCoders.Core;

[ExcludeFromCodeCoverage]
[PublicAPI]
public static class NullAction
{
    public static Action Instance { get; } = () => { };
}

[ExcludeFromCodeCoverage]
[PublicAPI]
public static class NullAction<T>
{
    public static Action<T> Instance { get; } = _ => { };
}
