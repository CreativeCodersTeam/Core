using System;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core.Collections;
using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.Blazor.Components.Base;

/// <summary>   Base class for building attributes for html elements. </summary>
[PublicAPI]
public class AttributeBuilder
{
    private bool _needsRebuild;

    private string _text;

    private readonly string _separatorForJoin;

    private readonly List<Func<string>> _items;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Initializes a new instance of the <see cref="AttributeBuilder"/> class. </summary>
    ///
    /// <param name="separatorForJoin">     The separator used for joining the parts of the
    ///                                     attribute. </param>
    /// <param name="separatorForSplit">    The separator for splitting the attributes in separate
    ///                                     parts. </param>
    ///-------------------------------------------------------------------------------------------------
    public AttributeBuilder(string separatorForJoin, string separatorForSplit)
    {
        _separatorForJoin = separatorForJoin;
        _items = new List<Func<string>>();

        _needsRebuild = true;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Adds an attribute part. </summary>
    ///
    /// <param name="attributePart">    The attribute part to add. </param>
    ///
    /// <returns>   This <see cref="AttributeBuilder"/>. </returns>
    ///-------------------------------------------------------------------------------------------------
    public AttributeBuilder Add(string attributePart)
    {
        return Add(() => attributePart);
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Adds a function returning an attribute part on rebuild. </summary>
    ///
    /// <param name="attributePart">    The function attribute part to add. </param>
    ///
    /// <returns>   This <see cref="AttributeBuilder"/>. </returns>
    ///-------------------------------------------------------------------------------------------------
    public AttributeBuilder Add(Func<string> attributePart)
    {
        _items.Add(attributePart);

        return this;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Adds an attribute part if condition is met. </summary>
    ///
    /// <param name="condition">        The function condition controlling if
    ///                                 <paramref name="attributePart"/> should be added. </param>
    /// <param name="attributePart">    The attribute part to add. </param>
    ///
    /// <returns>   This <see cref="AttributeBuilder"/>. </returns>
    ///-------------------------------------------------------------------------------------------------
    public AttributeBuilder AddIf(Func<bool> condition, string attributePart)
    {
        return Add(() => condition() ? attributePart : null);
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Adds an attribute part if condition is met. </summary>
    ///
    /// <param name="condition">        The function condition controlling if. </param>
    /// <param name="attributePart">    The function returning the attribute part to add. </param>
    ///
    /// <returns>   This <see cref="AttributeBuilder"/>. </returns>
    ///-------------------------------------------------------------------------------------------------
    public AttributeBuilder AddIf(Func<bool> condition, Func<string> attributePart)
    {
        return Add(() => condition() ? attributePart() : null);
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Builds the attribute text based on the added attribute parts. </summary>
    ///
    /// <returns>   A string. </returns>
    ///-------------------------------------------------------------------------------------------------
    public string Build()
    {
        var items =
            _items
                .Select(x => x())
                .WhereNotNull()
                .SelectMany(x => x.Split(_separatorForJoin, StringSplitOptions.RemoveEmptyEntries))
                .Distinct()
                .ToArray();

        return items.Any() ? string.Join(_separatorForJoin, items) : null;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Sets the need for rebuild. If called, the next call to <see cref="ToString"/> calls
    ///     <see cref="Build"/> again for recreating the text.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public void NeedsRebuild()
    {
        _needsRebuild = true;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Returns the string created by <see cref="Build"/>. The text is only build if needed,
    ///     otherwise the last value from build is returned.
    /// </summary>
    ///
    /// <returns>   A string that represents the current object. </returns>
    ///
    /// <seealso cref="System.Object.ToString()"/>
    ///-------------------------------------------------------------------------------------------------
    public override string ToString()
    {
        if (_needsRebuild)
        {
            _text = Build();
        }

        return _text;
    }
}
