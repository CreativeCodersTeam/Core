using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.SysConsole.CliArguments.Exceptions;
using CreativeCoders.SysConsole.CliArguments.Options;

namespace CreativeCoders.SysConsole.CliArguments.Parsing
{
    public class OptionParser
    {
        public object Parse(Type optionType, string[] args)
        {
            Ensure.NotNull(optionType, nameof(optionType));

            var option = Activator.CreateInstance(optionType);

            if (option == null)
            {
                throw new CliArgumentsException();
            }

            var optionArguments = ReadOptionArguments(args).ToArray();

            ReadOptionProperties(option).ForEach(x => x.Read(optionArguments, option));

            return option;
        }

        private IEnumerable<OptionArgument> ReadOptionArguments(string[] args)
        {
            var skipNext = false;

            var optionArguments = args.Select((x, index) =>
                {
                    if (skipNext)
                    {
                        skipNext = false;
                        return null;
                    }

                    if (!x.StartsWith("-", StringComparison.InvariantCulture))
                    {
                        return new OptionArgument
                        {
                            IsValue = true,
                            Value = x
                        };
                    }

                    string? value = null;

                    if (index < args.Length &&
                        !args[index + 1].StartsWith("-", StringComparison.InvariantCulture))
                    {
                        value = args[index + 1];
                        skipNext = true;
                    }

                    return new OptionArgument
                    {
                        IsShort = !x.StartsWith("--", StringComparison.InvariantCulture),
                        OptionName = x.TrimStart('-'),
                        Value = value
                    };

                })
                .Where(x => x != null)
                .ToArray();

            return optionArguments!;
        }

        private IEnumerable<OptionPropertyBase> ReadOptionProperties(object optionObject)
        {
            foreach (var propertyInfo in optionObject.GetType().GetProperties())
            {
                if (propertyInfo.GetCustomAttribute(typeof(OptionValueAttribute)) is OptionValueAttribute valueAttribute)
                {
                    yield return new ValueOptionProperty(propertyInfo, valueAttribute);
                }

                if (propertyInfo.GetCustomAttribute(typeof(OptionParameterAttribute)) is OptionParameterAttribute
                    optionAttribute)
                {
                    yield return new OptionProperty(propertyInfo, optionAttribute);
                }
            }
        }
    }

    public abstract class OptionPropertyBase
    {
        protected OptionPropertyBase(PropertyInfo propertyInfo)
        {
            Info = propertyInfo;
        }

        protected PropertyInfo Info { get; }

        public abstract bool Read(IEnumerable<OptionArgument> optionArguments, object optionObject);
    }

    public class OptionProperty : OptionPropertyBase
    {
        private readonly OptionParameterAttribute _optionParameterAttribute;

        public OptionProperty(PropertyInfo propertyInfo, OptionParameterAttribute optionParameterAttribute)
            : base(propertyInfo)
        {
            _optionParameterAttribute = optionParameterAttribute;
        }

        public override bool Read(IEnumerable<OptionArgument> optionArguments, object optionObject)
        {
            var optionArgument = optionArguments
                .FirstOrDefault(x =>
                    !x.IsValue && ((x.IsShort && x.OptionName == _optionParameterAttribute.ShortName.ToString()) ||
                                   (x.OptionName == _optionParameterAttribute.LongName)));

            if (optionArgument != null)
            {
                Info.SetValue(optionObject, optionArgument.Value);

                return true;
            }

            return false;
        }
    }

    public class ValueOptionProperty : OptionPropertyBase
    {
        private readonly OptionValueAttribute _optionValueAttribute;

        public ValueOptionProperty(PropertyInfo propertyInfo, OptionValueAttribute optionValueAttribute) : base(propertyInfo)
        {
            _optionValueAttribute = optionValueAttribute;
        }

        public override bool Read(IEnumerable<OptionArgument> optionArguments, object optionObject)
        {
            var optionArgument = optionArguments
                .Where(x => x.IsValue)
                .Skip(_optionValueAttribute.Index)
                .FirstOrDefault();

            if (optionArgument != null)
            {
                Info.SetValue(optionObject, optionArgument.Value);

                return true;
            }

            return false;
        }
    }

    public class OptionArgument
    {
        public bool IsValue { get; init; }

        public bool IsShort { get; init; }

        public string? OptionName { get; init; }

        public string? Value { get; init; }
    }
}
