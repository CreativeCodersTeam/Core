using System;
using System.Collections.Generic;
using System.Reflection;
using CreativeCoders.SysConsole.Cli.Parsing.Exceptions;

namespace CreativeCoders.SysConsole.Cli.Parsing.OptionProperties;

public abstract class OptionPropertyBase
{
    private readonly OptionBaseAttribute _optionAttribute;

    private readonly PropertyInfo _propertyInfo;

    protected OptionPropertyBase(PropertyInfo propertyInfo, OptionBaseAttribute optionAttribute)
    {
        _propertyInfo = propertyInfo;
        _optionAttribute = optionAttribute;
    }

    protected bool SetPropertyValue(OptionArgument? optionArgument, object optionObject)
    {
        if (optionArgument == null && _optionAttribute.DefaultValue == null)
        {
            return _optionAttribute.IsRequired
                ? throw new RequiredArgumentMissingException(_propertyInfo, _optionAttribute)
                : false;
        }

        var value = optionArgument == null
            ? _optionAttribute.DefaultValue
            : optionArgument.Value;

        var converter = (_optionAttribute.Converter != null
                            ? Activator.CreateInstance(_optionAttribute.Converter) as ICliValueConverter
                            : CliValueConverters.Default)
                        ?? CliValueConverters.Default;

        if (optionArgument != null)
        {
            optionArgument.IsProcessed = true;
        }

        var propertyValue = converter.Convert(value, _propertyInfo.PropertyType, _optionAttribute);

        if (propertyValue != ConverterAction.DoNothing)
        {
            _propertyInfo.SetValue(optionObject, propertyValue);
        }

        return true;
    }

    public abstract bool Read(IEnumerable<OptionArgument> optionArguments, object optionObject);
}
