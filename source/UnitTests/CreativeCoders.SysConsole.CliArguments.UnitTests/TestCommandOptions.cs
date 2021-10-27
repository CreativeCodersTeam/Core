using System;
using System.Security.Authentication.ExtendedProtection;
using CreativeCoders.SysConsole.CliArguments.Options;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests
{
    public class TestCommandOptions
    {
        [OptionParameter('t', "text")]
        public string Text { get; set; }
    }
}
