using CreativeCoders.Core.Enums;
using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.Blazor.Components.Buttons;

[PublicAPI]
[EnumStringValue(null)]
public enum ButtonType
{
    Button,
    [EnumStringValue("submit")]
    Submit,
    [EnumStringValue("reset")]
    Reset
}