using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;

namespace CreativeCoders.AspNetCore.Blazor.Components.Base;

/// -------------------------------------------------------------------------------------------------
/// <summary>   Base class for a blazor control. </summary>
/// <seealso cref="ComponentBase" />
/// -------------------------------------------------------------------------------------------------
[PublicAPI]
public class ControlBase : ComponentBase
{
    protected override void OnInitialized()
    {
        base.OnInitialized();

        SetupClasses();

        SetupStyles();
    }

    protected virtual void SetupClasses()
    {
        Classes.Add(() =>
            CustomAttributes != null && CustomAttributes.TryGetValue("class", out var className)
                ? className?.ToString()
                : null);
    }

    protected virtual void SetupStyles()
    {
        Styles.Add(() =>
            CustomAttributes != null && CustomAttributes.TryGetValue("style", out var styleName)
                ? styleName?.ToString()
                : null);
    }

    public ClassesAttributeBuilder Classes { get; } = new ClassesAttributeBuilder();

    public StyleAttributeBuilder Styles { get; } = new StyleAttributeBuilder();

    [Parameter] public string Id { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object> CustomAttributes { get; set; }
}
