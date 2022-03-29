using System;
using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.Blazor.Components.Base;

///-------------------------------------------------------------------------------------------------
/// <summary>   The attribute builder for the style attribute on html elements. </summary>
///
/// <seealso cref="AttributeBuilder"/>
///-------------------------------------------------------------------------------------------------
[PublicAPI]
public class StyleAttributeBuilder : AttributeBuilder
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="StyleAttributeBuilder"/> class.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public StyleAttributeBuilder() : base(" ", ";") { }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Adds a property. </summary>
    ///
    /// <param name="propertyName"> Name of the property. </param>
    /// <param name="value">        The value. </param>
    ///
    /// <returns>   This <see cref="StyleAttributeBuilder"/>. </returns>
    ///-------------------------------------------------------------------------------------------------
    public StyleAttributeBuilder AddProperty(string propertyName, string value)
    {
        Add($"{propertyName}: {value};");

        return this;
    }

    public StyleAttributeBuilder AddProperty(Func<string> propertyName, Func<string> value)
    {
        Add(() => $"{propertyName()}: {value()};");

        return this;
    }
}
