using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core;

public class ConsoleArguments
{
    private readonly bool _caseSensitive;

    private readonly IList<string> _params;

    private readonly IDictionary<string, string> _values;

    public ConsoleArguments(IEnumerable<string> args, bool caseSensitive)
    {
        Ensure.IsNotNull(args, "args");

        _params = new List<string>();
        _values = new Dictionary<string, string>();
        _caseSensitive = caseSensitive;

        ReadArguments(args);
    }

    public string this[string valueName]
    {
        get
        {
            if (valueName.Substring(0, 1) != "/")
            {
                valueName = "/" + valueName;
            }

            var result = string.Empty;
            var (_, value) = _values.FirstOrDefault(x => ValueNameEquals(valueName, x.Key));
            if (!string.IsNullOrWhiteSpace(value))
            {
                result = value;
            }

            return result;
        }
    }

    public bool IsSet(string valueName)
    {
        return _params.Any(x => ValueNameEquals(valueName, x));
    }

    public IEnumerable<string> Params => _params;

    public IEnumerable<string> Values => _values.Select(x => x.Key);

    private void ReadArguments(IEnumerable<string> args)
    {
        foreach (var arg in args.Where(x => !string.IsNullOrWhiteSpace(x)))
        {
            if (arg.StartsWith("/", StringComparison.Ordinal))
            {
                ExtractArgument(arg);
            }
            else
            {
                _params.Add(arg);
            }
        }
    }

    private void ExtractArgument(string arg)
    {
        var index = arg.IndexOf("=", StringComparison.Ordinal);
        if (index == -1)
        {
            index = arg.IndexOf(":", StringComparison.Ordinal);
        }

        if (index == -1)
        {
            _values[arg] = string.Empty;
        }
        else
        {
            _values[arg.Substring(0, index)] = arg.Substring(index + 1);
        }
    }

    private bool ValueNameEquals(string valueName, string value)
    {
        return _caseSensitive ? valueName.Equals(value) : valueName.ToLower().Equals(value.ToLower());
    }
}
