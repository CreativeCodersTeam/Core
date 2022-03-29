using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CreativeCoders.SysConsole.Cli.Parsing.Properties;

public class OptionParameterProperty : OptionPropertyBase
{
    private readonly OptionParameterAttribute _optionParameterAttribute;

    public OptionParameterProperty(PropertyInfo propertyInfo,
        OptionParameterAttribute optionParameterAttribute)
        : base(propertyInfo, optionParameterAttribute)
    {
        _optionParameterAttribute = optionParameterAttribute;
    }

    public override bool Read(IEnumerable<OptionArgument> optionArguments, object optionObject)
    {
        var optionArgument = optionArguments
            .FirstOrDefault(x =>
                x.Kind == OptionArgumentKind.ShortName
                && x.OptionName == _optionParameterAttribute.ShortName.ToString() ||
                x.Kind == OptionArgumentKind.LongName && x.OptionName == _optionParameterAttribute.LongName);

        return SetPropertyValue(optionArgument, optionObject);
    }
}
