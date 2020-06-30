using Microsoft.AspNetCore.Components;

namespace CreativeCoders.AspNetCore.Blazor.Components.Base
{
    public class ContainerControlBase : ControlBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
    }
}