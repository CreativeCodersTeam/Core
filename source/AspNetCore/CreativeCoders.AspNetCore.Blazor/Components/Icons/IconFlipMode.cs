using System;
using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.Blazor.Components.Icons;

[PublicAPI]
[Flags]
public enum IconFlipMode
{
    None,
    Vertical,
    Horizontal
}