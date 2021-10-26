using System.Collections.Generic;
using System.Reflection;

namespace CreativeCoders.SysConsole.CliArguments.Parsing.Properties
{
    public abstract class OptionPropertyBase
    {
        protected OptionPropertyBase(PropertyInfo propertyInfo)
        {
            Info = propertyInfo;
        }

        protected PropertyInfo Info { get; }

        public abstract bool Read(IEnumerable<OptionArgument> optionArguments, object optionObject);
    }
}
