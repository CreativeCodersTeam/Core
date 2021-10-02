using CreativeCoders.AspNetCore.Blazor.Components.Base;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace CreativeCoders.AspNetCore.Blazor.Components.Icons
{
    [PublicAPI]
    public class Iconify : ControlBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            var sequence = 0;
            
            builder.OpenElement(sequence++, "div");
            builder.AddAttribute(sequence++, "id", Id);

            builder.OpenElement(sequence++, "span");
            builder.AddAttribute(sequence++, "class", "iconify");
            builder.AddAttribute(sequence++, "data-icon", Icon);
            builder.AddAttribute(sequence++, "data-inline", "false");

            if (Width.IsNotNullOrWhiteSpace())
            {
                builder.AddAttribute(sequence++, "data-width", Width);
            }

            if (Height.IsNotNullOrWhiteSpace())
            {
                builder.AddAttribute(sequence++, "data-height", Height);
            }

            if (Rotation != 0)
            {
                builder.AddAttribute(sequence++, "data-rotate", $"{Rotation}deg");
            }

            if (!IsInline)
            {
                builder.AddAttribute(sequence++, "data-inline", "false");
            }

            builder.AddAttribute(sequence++, "style", Styles.ToString());

            builder.AddMultipleAttributes(sequence, CustomAttributes);

            builder.CloseElement();
            builder.CloseElement();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (Color.IsNotNullOrWhiteSpace())
            {
                Styles.AddProperty("color", Color);
            }
        }

        [Parameter] public string Icon { get; set; }

        [Parameter] public string Width { get; set; }

        [Parameter] public string Height { get; set; }
        
        [Parameter] public string Color { get; set; }
        
        [Parameter] public int Rotation { get; set; }
        
        [Parameter] public IconFlipMode FlipMode { get; set; }

        [Parameter] public bool IsInline { get; set; }
    }
}
