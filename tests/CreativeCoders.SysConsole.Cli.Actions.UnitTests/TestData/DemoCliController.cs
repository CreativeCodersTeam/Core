﻿using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.SysConsole.Cli.Actions.Definition;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData;

[PublicAPI]
[CliController("demo")]
public class DemoCliController
{
    [CliAction("test")]
    public Task<CliActionResult> DoAsync()
    {
        return Task.FromResult(new CliActionResult());
    }

    [CliAction("more/command")]
    public Task<CliActionResult> DoMoreAsync(DoCmdOptions options)
    {
        Ensure.NotNull(options, nameof(options));

        return Task.FromResult(new CliActionResult());
    }
}
