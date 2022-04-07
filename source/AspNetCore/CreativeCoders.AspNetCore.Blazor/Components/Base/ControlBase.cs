using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;

namespace CreativeCoders.AspNetCore.Blazor.Components.Base;

///-------------------------------------------------------------------------------------------------
/// <summary>   Base class for a blazor control. </summary>
///
/// <seealso cref="ComponentBase"/>
///-------------------------------------------------------------------------------------------------
[PublicAPI]
public class ControlBase : ComponentBase
{
    public ControlBase()
    {
        Classes = new ClassesAttributeBuilder();
        Styles = new StyleAttributeBuilder();
    }

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

    public ClassesAttributeBuilder Classes { get; }

    public StyleAttributeBuilder Styles { get; }

    [Parameter] public string Id { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object> CustomAttributes { get; set; }
}
