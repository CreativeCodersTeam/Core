using CreativeCoders.Core.Enums;
using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.Blazor.Components.Buttons;

[PublicAPI]
[EnumStringValue(null)]
public enum ButtonSize
{
    Normal,
    [EnumStringValue("btn-sm")]
    Small,
    [EnumStringValue("btn-lg")]
    Large
}