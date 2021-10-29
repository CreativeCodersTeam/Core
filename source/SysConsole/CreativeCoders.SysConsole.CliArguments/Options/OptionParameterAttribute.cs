﻿using System;

namespace CreativeCoders.SysConsole.CliArguments.Options
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class OptionParameterAttribute : OptionBaseAttribute
    {
        public OptionParameterAttribute(char shortName, string longName)
        {
            ShortName = shortName;
            LongName = longName;
        }

        public char ShortName { get; }

        public string LongName { get; }
    }
}
