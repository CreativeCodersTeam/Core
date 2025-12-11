using System.Reflection;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting.Commands;
using CreativeCoders.Cli.Hosting.Commands.Store;
using CreativeCoders.Cli.Hosting.Exceptions;
using CreativeCoders.Cli.Hosting.Help;
using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;
using CreativeCoders.SysConsole.Cli.Parsing;
using Spectre.Console;

namespace CreativeCoders.Cli.Hosting;

public class DefaultCliHost(
    IAnsiConsole ansiConsole,
    ICliCommandStore commandStore,
    IServiceProvider serviceProvider,
    ICliCommandHelpHandler commandHelpHandler)
    : ICliHost
{
    private readonly IServiceProvider _serviceProvider = Ensure.NotNull(serviceProvider);

    private readonly ICliCommandStore _commandStore = Ensure.NotNull(commandStore);

    private readonly IAnsiConsole _ansiConsole = Ensure.NotNull(ansiConsole);

    private readonly ICliCommandHelpHandler _commandHelpHandler = Ensure.NotNull(commandHelpHandler);

    private (object Command, string[] Args, CliCommandInfo CommandInfo) CreateCliCommand(string[] args)
    {
        var findCommandNodeResult = _commandStore.FindCommandNode(args);

        if (findCommandNodeResult?.Node?.CommandInfo == null)
        {
            throw new CliCommandNotFoundException("No command found for given args", args);
        }

        try
        {
            var commandInfo = findCommandNodeResult.Node?.CommandInfo ??
                              throw new InvalidOperationException("CommandInfo must not be null");
            var command =
                commandInfo.CommandType.CreateInstance<object>(_serviceProvider);

            return command == null
                ? throw new CliCommandConstructionFailedException("Command creation failed", args)
                : (command, findCommandNodeResult.RemainingArgs, commandInfo);
        }
        catch (Exception e)
        {
            throw new CliCommandConstructionFailedException("Command creation failed", args, e);
        }
    }

    private static async Task<CliResult> ExecuteAsync(CliCommandInfo commandInfo, object command,
        string[] optionsArgs)
    {
        if (commandInfo.OptionsType == null)
        {
            var commandResult = await ((ICliCommand)command).ExecuteAsync().ConfigureAwait(false);

            return new CliResult(commandResult.ExitCode);
        }

        var optionsParser = new OptionParser(commandInfo.OptionsType);

        var options = optionsParser.Parse(optionsArgs);

        var commandWithOptionsResult = command.GetType().InvokeMember(nameof(ICliCommand<>.ExecuteAsync),
            BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, command,
            [options]);

        if (commandWithOptionsResult is Task<CommandResult> taskResult)
        {
            return new CliResult((await taskResult.ConfigureAwait(false)).ExitCode);
        }

        return new CliResult(int.MaxValue);
    }

    public async Task<CliResult> RunAsync(string[] args)
    {
        try
        {
            if (_commandHelpHandler.ShouldPrintHelp(args))
            {
                _commandHelpHandler.PrintHelp(args);

                return new CliResult(CliExitCodes.Success);
            }

            var (command, optionsArgs, commandInfo) = CreateCliCommand(args);

            return await ExecuteAsync(commandInfo, command, optionsArgs).ConfigureAwait(false);
        }
        catch (CliCommandConstructionFailedException e)
        {
            _ansiConsole.Markup(
                $"[red]Error creating command: {e.InnerException?.Message ?? "Unknown error"}[/] ");

            return new CliResult(e.ExitCode);
        }
        catch (CliCommandNotFoundException e)
        {
            PrintNearestMatch(args);

            return new CliResult(e.ExitCode);
        }
    }

    private void PrintNearestMatch(string[] args)
    {
        _ansiConsole.Markup($"[red]No command found for given arguments: {string.Join(" ", args)}[/]");
        _ansiConsole.WriteLine();
        _ansiConsole.WriteLine("Suggestions:");

        var findCommandGroupNodeResult = _commandStore.FindCommandGroupNode(args);

        if (findCommandGroupNodeResult?.Node == null)
        {
            _ansiConsole.WriteLine("No matches found");

            return;
        }

        _commandHelpHandler.PrintHelpFor(findCommandGroupNodeResult.Node.ChildNodes);
    }
}
