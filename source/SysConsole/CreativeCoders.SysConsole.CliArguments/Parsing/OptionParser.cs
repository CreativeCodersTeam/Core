using System;
using System.Collections.Generic;
using System.Reflection;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.SysConsole.CliArguments.Exceptions;
using CreativeCoders.SysConsole.CliArguments.Options;
using CreativeCoders.SysConsole.CliArguments.Parsing.Properties;

namespace CreativeCoders.SysConsole.CliArguments.Parsing
{
    public class OptionParser
    {
        public object Parse(Type optionType, string[] args)
        {
            Ensure.NotNull(optionType, nameof(optionType));

            object? option;

            try
            {
                option = Activator.CreateInstance(optionType);

                if (option == null)
                {
                    throw new OptionCreationFailedException(optionType);
                }
            }
            catch (Exception e)
            {
                throw new OptionCreationFailedException(optionType, e);
            }

            var optionArguments = new ArgsToOptionArgumentsConverter(args).ReadOptionArguments();

            ReadOptionProperties(option).ForEach(x =>
            {
                if (!x.Read(optionArguments, option))
                {
                    
                }
            });

            return option;
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
                    yield return new OptionParameterProperty(propertyInfo, optionAttribute);
                }
            }
        }
    }
}
