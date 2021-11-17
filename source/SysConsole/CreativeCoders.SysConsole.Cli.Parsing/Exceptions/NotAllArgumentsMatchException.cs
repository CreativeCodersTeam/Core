using System;
using System.Collections.Generic;

namespace CreativeCoders.SysConsole.Cli.Parsing.Exceptions
{
    public class NotAllArgumentsMatchException : Exception
    {
        public NotAllArgumentsMatchException(IEnumerable<OptionArgument> notMatchedArgs)
        {
            NotMatchedArgs = notMatchedArgs;
        }

        public IEnumerable<OptionArgument> NotMatchedArgs { get; }
    }
}
