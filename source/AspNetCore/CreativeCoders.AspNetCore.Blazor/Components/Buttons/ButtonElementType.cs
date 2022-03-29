using CreativeCoders.Core.Enums;
using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.Blazor.Components.Buttons;

[PublicAPI]
[EnumStringValue("button")]
public enum ButtonElementType
{
    Button,
    [EnumStringValue("a")]
    Link,
    [EnumStringValue("div")]
    Div
}