using System.Reflection;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting.Commands.Store;
using CreativeCoders.Cli.Hosting.Exceptions;
using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;
using CreativeCoders.SysConsole.Cli.Parsing;

namespace CreativeCoders.Cli.Hosting;

public class DefaultCliHost(ICliCommandStore commandStore, IServiceProvider serviceProvider)
    : ICliHost
{
    private readonly IServiceProvider _serviceProvider = Ensure.NotNull(serviceProvider);

    private readonly ICliCommandStore _commandStore = Ensure.NotNull(commandStore);

    private (object? Command, string[] Args) CreateCliCommand(string[] args)
    {
        var findCommandNodeResult = _commandStore.FindCommandNode(args);

        if (findCommandNodeResult?.Node?.CommandInfo == null)
        {
            throw new CliCommandNotFoundException("No command found for given args", args);
        }

        var command =
            findCommandNodeResult.Node?.CommandInfo.CommandType.CreateInstance<object>(_serviceProvider);

        return command == null
            ? throw new CliCommandConstructionFailedException("Command creation failed", args)
            : (command, findCommandNodeResult.RemainingArgs);
    }

    private static async Task<CliResult> ExecuteAsync(object command, string[] optionsArgs)
    {
        if (command.GetType().IsAssignableTo(typeof(ICliCommand)))
        {
            var commandResult = await ((ICliCommand)command).ExecuteAsync().ConfigureAwait(false);

            return new CliResult(commandResult.ExitCode);
        }

        if (!command.GetType().ImplementsGenericInterface(typeof(ICliCommand<>)))
        {
            return new CliResult(int.MinValue);
        }

        var optionsTypes = command.GetType().GetGenericInterfaceArguments(typeof(ICliCommand<>));

        if (optionsTypes.Length != 1)
        {
            throw new InvalidOperationException("Invalid command type");
        }

        var optionsParser = new OptionParser(optionsTypes[0]);

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
        var (command, optionsArgs) = CreateCliCommand(args);

        if (command == null)
        {
            throw new InvalidOperationException("No command found");
        }

        return await ExecuteAsync(command, optionsArgs).ConfigureAwait(false);
    }
}
