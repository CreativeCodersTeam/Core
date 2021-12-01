using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.SysConsole.Cli.Parsing.Exceptions;
using CreativeCoders.SysConsole.Cli.Parsing.Properties;

namespace CreativeCoders.SysConsole.Cli.Parsing
{
    public class OptionParser
    {
        private readonly Type _optionType;

        public OptionParser(Type optionType)
        {
            _optionType = Ensure.NotNull(optionType, nameof(optionType));
        }

        public object Parse(string[] args)
        {
            Ensure.NotNull(_optionType, nameof(_optionType));

            object? option;

            try
            {
                option = Activator.CreateInstance(_optionType);

                if (option == null)
                {
                    throw new OptionCreationFailedException(_optionType);
                }
            }
            catch (Exception e)
            {
                throw new OptionCreationFailedException(_optionType, e);
            }

            var optionArguments = new ArgsToOptionArgumentsConverter(args).ReadOptionArguments();

            ReadOptionProperties().ForEach(x =>
            {
                if (!x.Read(optionArguments, option))
                {
                    
                }
            });

            CheckAllArguments(optionArguments, option);

            return option;
        }

        private static void CheckAllArguments(IEnumerable<OptionArgument> optionArguments, object option)
        {
            var optionsAttribute =
                option.GetType().GetCustomAttribute<OptionsAttribute>();

            if (optionsAttribute?.AllArgsMustMatch != true)
            {
                return;
            }

            var notMatchedArgs = optionArguments.Where(x => !x.IsProcessed).ToArray();

            if (notMatchedArgs.Any())
            {
                throw new NotAllArgumentsMatchException(notMatchedArgs);
            }
        }
        
        private IEnumerable<OptionPropertyBase> ReadOptionProperties()
        {
            foreach (var propertyInfo in _optionType.GetProperties())
            {
                var valueAttribute = propertyInfo.GetCustomAttribute<OptionValueAttribute>();
                if (valueAttribute != null)
                {
                    yield return new ValueOptionProperty(propertyInfo, valueAttribute);
                }

                var optionAttribute = propertyInfo.GetCustomAttribute<OptionParameterAttribute>();
                if (optionAttribute != null)
                {
                    yield return new OptionParameterProperty(propertyInfo, optionAttribute);
                }
            }
        }
    }
}
