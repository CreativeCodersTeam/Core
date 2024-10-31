using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.SysConsole.Cli.Parsing;

public class ArgsToOptionArgumentsConverter(string[] args)
{
    public IEnumerable<OptionArgument> ReadOptionArguments()
    {
        var skipNext = false;

        var optionArguments = args.Select((arg, index) =>
            {
                if (skipNext)
                {
                    skipNext = false;
                    return null;
                }

                var argumentKind = GetArgumentKind(arg);

                if (argumentKind == OptionArgumentKind.Value)
                {
                    return new OptionArgument { Kind = argumentKind, Value = arg };
                }

                return new OptionArgument
                {
                    Kind = argumentKind,
                    OptionName = arg.TrimStart('-'),
                    Value = GetParameterValue(index, ref skipNext)
                };
            })
            .Where(x => x != null)
            .ToArray();

        return optionArguments!;
    }

    private string? GetParameterValue(int index, ref bool skipNext)
    {
        if (index + 1 >= args.Length || args[index + 1].StartsWith('-'))
        {
            return null;
        }

        var value = args[index + 1];
        skipNext = true;

        return value;
    }

    private static OptionArgumentKind GetArgumentKind(string arg)
    {
        if (arg.StartsWith("--", StringComparison.Ordinal))
        {
            return OptionArgumentKind.LongName;
        }

        return arg.StartsWith('-')
            ? OptionArgumentKind.ShortName
            : OptionArgumentKind.Value;
    }
}
