using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core;

namespace CreativeCoders.SysConsole.Cli.Parsing.OptionProperties;

public class OptionParameterProperty(
    PropertyInfo propertyInfo,
    OptionParameterAttribute optionParameterAttribute)
    : OptionPropertyBase(propertyInfo, optionParameterAttribute)
{
    private readonly OptionParameterAttribute _optionParameterAttribute =
        Ensure.NotNull(optionParameterAttribute);

    public override bool Read(IEnumerable<OptionArgument> optionArguments, object optionObject)
    {
        var optionArgument = optionArguments
            .FirstOrDefault(x =>
                (x.Kind == OptionArgumentKind.ShortName
                 && x.OptionName == _optionParameterAttribute.ShortName) ||
                (x.Kind == OptionArgumentKind.LongName &&
                 x.OptionName == _optionParameterAttribute.LongName));

        return SetPropertyValue(optionArgument, optionObject);
    }
}
