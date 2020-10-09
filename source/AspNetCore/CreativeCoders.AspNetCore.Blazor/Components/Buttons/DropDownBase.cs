using System.Collections.Generic;
using CreativeCoders.AspNetCore.Blazor.Components.Base;
using Microsoft.AspNetCore.Components;

namespace CreativeCoders.AspNetCore.Blazor.Components.Buttons
{
    public class DropDownBase<T> : ControlBase
    {
        protected override void SetupClasses()
        {
            base.SetupClasses();

            Classes.Add("btn");
            Classes.Add("dropdown-toggle");
        }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public IReadOnlyCollection<T> DropDownItems { get; set; }

        [Parameter] public RenderFragment ButtonTemplate { get; set; }

        [Parameter] public RenderFragment<T> DropDownItemTemplate { get; set; }
    }
}