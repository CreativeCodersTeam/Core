using CreativeCoders.AspNetCore.Blazor.Components.Base;
using CreativeCoders.Core.Enums;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CreativeCoders.AspNetCore.Blazor.Components.Buttons
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A base class for a button. </summary>
    ///
    /// <seealso cref="ContainerControlBase"/>
    ///-------------------------------------------------------------------------------------------------
    [PublicAPI]
    public class ButtonBase : ContainerControlBase
    {
        private const string ButtonClassPrefix = "btn-";

        private const string OutlinedButtonClassPrefix = "btn-outline-";


        /// <inheritdoc/>>
        protected override void SetupClasses()
        {
            base.SetupClasses();

            Classes
                .Add("btn")
                .Add(() => (IsOutlined ? OutlinedButtonClassPrefix : ButtonClassPrefix) + Kind.ToText())
                .Add(() => Size.ToText());
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the click event callback. </summary>
        ///
        /// <value> The callback for the click event. </value>
        ///-------------------------------------------------------------------------------------------------
        [Parameter] public EventCallback<MouseEventArgs> Clicked { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the kind of the button. </summary>
        ///
        /// <value> The kind of the button. </value>
        ///-------------------------------------------------------------------------------------------------
        [Parameter] public ButtonKind Kind { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the size. </summary>
        ///
        /// <value> The size. </value>
        ///-------------------------------------------------------------------------------------------------
        [Parameter]
        public ButtonSize Size { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this button is outlined. </summary>
        ///
        /// <value> True if this button is outlined, false if not. </value>
        ///-------------------------------------------------------------------------------------------------
        [Parameter] public bool IsOutlined { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this button is block level. </summary>
        ///
        /// <value> True if this button is block level, false if not. </value>
        ///-------------------------------------------------------------------------------------------------
        [Parameter] public bool IsBlockLevel { get; set; }
    }
}