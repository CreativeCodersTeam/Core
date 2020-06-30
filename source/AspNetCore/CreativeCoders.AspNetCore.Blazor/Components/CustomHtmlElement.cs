using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace CreativeCoders.AspNetCore.Blazor.Components
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Renders a custom html tag. </summary>
    ///
    /// <seealso cref="ComponentBase"/>
    ///-------------------------------------------------------------------------------------------------
    [PublicAPI]
    public class CustomHtmlElement : ComponentBase
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the tag. </summary>
        ///
        /// <value> The tag used for the rendered html element. </value>
        ///-------------------------------------------------------------------------------------------------
        [Parameter]
        public string Tag { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the child content. </summary>
        ///
        /// <value> The child content rendered inside the custom html element. </value>
        ///-------------------------------------------------------------------------------------------------
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the custom attributes. </summary>
        ///
        /// <value> Captures all other defined attributes specified in the html element. </value>
        ///-------------------------------------------------------------------------------------------------
        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> CustomAttributes { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the on click prevent default. </summary>
        ///
        /// <value> True if on click prevent default, false if not. </value>
        ///-------------------------------------------------------------------------------------------------
        [Parameter]
        public bool OnClickPreventDefault { get; set; }

        [Parameter] public bool OnClickStopPropagation { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Renders the component to the supplied
        ///     <see cref="T:Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder" />.
        /// </summary>
        ///
        /// <param name="builder">  A <see cref="T:Microsoft.AspNetCore.Components.Rendering.RenderTreeBuil
        ///                         der" /> that will receive the render output. </param>
        ///
        /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase.BuildRenderTree(RenderTreeBuilder)"/>
        ///-------------------------------------------------------------------------------------------------
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.OpenElement(0, Tag);

            builder.AddMultipleAttributes(1, CustomAttributes);
            builder.AddEventPreventDefaultAttribute(2, "onclick", OnClickPreventDefault);
            builder.AddEventStopPropagationAttribute(3, "onclick", OnClickStopPropagation);

            builder.AddContent(4, ChildContent);

            builder.CloseElement();
        }
    }
}