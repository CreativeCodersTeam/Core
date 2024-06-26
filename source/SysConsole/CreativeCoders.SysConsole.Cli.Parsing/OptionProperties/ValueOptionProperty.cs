﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CreativeCoders.SysConsole.Cli.Parsing.OptionProperties;

public class ValueOptionProperty : OptionPropertyBase
{
    private readonly OptionValueAttribute _optionValueAttribute;

    public ValueOptionProperty(PropertyInfo propertyInfo, OptionValueAttribute optionValueAttribute)
        : base(propertyInfo, optionValueAttribute)
    {
        _optionValueAttribute = optionValueAttribute;
    }

    public override bool Read(IEnumerable<OptionArgument> optionArguments, object optionObject)
    {
        var optionArgument = optionArguments
            .Where(x => x.Kind == OptionArgumentKind.Value)
            .Skip(_optionValueAttribute.Index)
            .FirstOrDefault();

        return SetPropertyValue(optionArgument, optionObject);
    }
}
