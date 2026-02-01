using System.Reflection;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting.Commands;
using CreativeCoders.Cli.Hosting.Commands.Store;
using CreativeCoders.Cli.Hosting.Exceptions;
using CreativeCoders.Cli.Hosting.Help;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.Reflection;
using CreativeCoders.SysConsole.Cli.Parsing;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace CreativeCoders.Cli.Hosting;

public class DefaultCliHost(
    IAnsiConsole ansiConsole,
    ICliCommandStore commandStore,
    IServiceProvider serviceProvider,
    ICliCommandHelpHandler commandHelpHandler,
    IEnumerable<ICliPreProcessor> preProcessors,
    IEnumerable<ICliPostProcessor> postProcessors)
    : ICliHost
{
    private readonly IServiceProvider _serviceProvider = Ensure.NotNull(serviceProvider);

    private readonly ICliCommandStore _commandStore = Ensure.NotNull(commandStore);

    private readonly IAnsiConsole _ansiConsole = Ensure.NotNull(ansiConsole);

    private readonly ICliCommandHelpHandler _commandHelpHandler = Ensure.NotNull(commandHelpHandler);

    private readonly CliHostSettings _settings =
        serviceProvider.GetService<CliHostSettings>() ?? new CliHostSettings();

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

            return command is null
                ? throw new CliCommandConstructionFailedException("Command creation failed", args)
                : (command, findCommandNodeResult.RemainingArgs, commandInfo);
        }
        catch (Exception e)
        {
            throw new CliCommandConstructionFailedException("Command creation failed", args, e);
        }
    }

    private async Task<CliResult> ExecuteAsync(CliCommandInfo commandInfo, object command,
        string[] optionsArgs)
    {
        if (commandInfo.OptionsType is null)
        {
            var commandResult = await ((ICliCommand)command).ExecuteAsync().ConfigureAwait(false);

            return new CliResult(commandResult.ExitCode);
        }

        var optionsParser = new OptionParser(commandInfo.OptionsType);

        var options = optionsParser.Parse(optionsArgs);
        await ValidateCommandOptionsAsync(options).ConfigureAwait(false);

        var commandWithOptionsResult = command.GetType().InvokeMember(nameof(ICliCommand<>.ExecuteAsync),
            BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, command,
            [options]);

        if (commandWithOptionsResult is Task<CommandResult> taskResult)
        {
            return new CliResult((await taskResult.ConfigureAwait(false)).ExitCode);
        }

        return new CliResult(CliExitCodes.CommandResultUnknown);
    }

    private async Task ValidateCommandOptionsAsync(object options)
    {
        if (!_settings.UseValidation)
        {
            return;
        }

        if (options is not IOptionsValidation validator)
        {
            return;
        }

        var validationResult = await validator.ValidateAsync().ConfigureAwait(false);

        if (!validationResult.IsValid)
        {
            throw new CliCommandOptionsInvalidException(validationResult);
        }
    }

    public async Task<CliResult> RunAsync(string[] args)
    {
        try
        {
            if (_commandHelpHandler.ShouldPrintHelp(args))
            {
                await ExecuteHelpPreProcessorsAsync(args).ConfigureAwait(false);

                _commandHelpHandler.PrintHelp(args);

                var cliHelpResult = new CliResult(CliExitCodes.Success);

                return await ExecuteHelpPostProcessorsAsync(cliHelpResult).ConfigureAwait(false);
            }

            await ExecuteCommandPreProcessorsAsync(args).ConfigureAwait(false);

            var (command, optionsArgs, commandInfo) = CreateCliCommand(args);

            var commandContext = _serviceProvider.GetRequiredService<ICliCommandContext>();
            commandContext.AllArgs = args;
            commandContext.OptionsArgs = optionsArgs;

            var cliResult = await ExecuteAsync(commandInfo, command, optionsArgs).ConfigureAwait(false);

            return await ExecuteCommandPostProcessorsAsync(cliResult).ConfigureAwait(false);
        }
        catch (CliCommandConstructionFailedException e)
        {
            if (e.InnerException is CliCommandAbortException abortException)
            {
                return HandleCommandAbortException(abortException);
            }

            _ansiConsole.Markup(
                $"[red]Error creating command: {e.InnerException?.Message ?? "Unknown error"}[/] ");

            return new CliResult(e.ExitCode);
        }
        catch (CliCommandNotFoundException e)
        {
            PrintNearestMatch(args);

            return new CliResult(e.ExitCode);
        }
        catch (CliCommandOptionsInvalidException e)
        {
            _ansiConsole.MarkupLine("[red]Error validating command options[/]");

            e.ValidationResult.Messages.ForEach(message => _ansiConsole.MarkupLine($"- {message}"));

            return new CliResult(e.ExitCode);
        }
        catch (CliCommandAbortException e)
        {
            return HandleCommandAbortException(e);
        }
        catch (CliExitException e)
        {
            _ansiConsole.MarkupLine($"[red]{e.Message}[/]");

            return new CliResult(e.ExitCode);
        }
    }

    private CliResult HandleCommandAbortException(CliCommandAbortException e)
    {
        if (e.PrintMessage)
        {
            _ansiConsole.MarkupLine(e.IsError ? $"[red]{e.Message}[/]" : $"[yellow]{e.Message}[/]");
        }

        return new CliResult(e.ExitCode);
    }

    private async Task ExecuteHelpPreProcessorsAsync(string[] args)
    {
        CliProcessorExecutionCondition[] conditions =
            [CliProcessorExecutionCondition.OnlyOnHelp, CliProcessorExecutionCondition.Always];

        await ExecutePreProcessorsAsync(preProcessors, conditions, args)
            .ConfigureAwait(false);
    }

    private async Task ExecuteCommandPreProcessorsAsync(string[] args)
    {
        CliProcessorExecutionCondition[] conditions =
            [CliProcessorExecutionCondition.OnlyOnCommand, CliProcessorExecutionCondition.Always];

        await ExecutePreProcessorsAsync(preProcessors, conditions, args)
            .ConfigureAwait(false);
    }

    private static async Task ExecutePreProcessorsAsync(IEnumerable<ICliPreProcessor> preProcessors,
        CliProcessorExecutionCondition[] conditions, string[] args)
    {
        foreach (var preProcessor in preProcessors.Where(x => conditions.Contains(x.ExecutionCondition)))
        {
            try
            {
                await preProcessor.ExecuteAsync(args).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new CliPreProcessorException(preProcessor,
                    $"PreProcessor execution failed: {e.Message}", e);
            }
        }
    }

    private async Task<CliResult> ExecuteCommandPostProcessorsAsync(CliResult cliResult)
    {
        CliProcessorExecutionCondition[] conditions =
            [CliProcessorExecutionCondition.OnlyOnCommand, CliProcessorExecutionCondition.Always];

        return await ExecutePostProcessorsAsync(postProcessors, conditions, cliResult).ConfigureAwait(false);
    }

    private async Task<CliResult> ExecuteHelpPostProcessorsAsync(CliResult cliResult)
    {
        CliProcessorExecutionCondition[] conditions =
            [CliProcessorExecutionCondition.OnlyOnHelp, CliProcessorExecutionCondition.Always];

        return await ExecutePostProcessorsAsync(postProcessors, conditions, cliResult).ConfigureAwait(false);
    }

    private static async Task<CliResult> ExecutePostProcessorsAsync(
        IEnumerable<ICliPostProcessor> postProcessors, CliProcessorExecutionCondition[] conditions,
        CliResult cliResult)
    {
        foreach (var postProcessor in postProcessors.Where(x => conditions.Contains(x.ExecutionCondition)))
        {
            try
            {
                await postProcessor.ExecuteAsync(cliResult).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new CliPostProcessorException(postProcessor,
                    $"PostProcessor execution failed: {e.Message}", e);
            }
        }

        return cliResult;
    }

    /// <inheritdoc />
    public async Task<int> RunMainAsync(string[] args)
    {
        var result = await RunAsync(args).ConfigureAwait(false);

        return result.ExitCode;
    }

    private void PrintNearestMatch(string[] args)
    {
        _ansiConsole.Markup($"[red]No command found for given arguments: {string.Join(" ", args)}[/]");
        _ansiConsole.WriteLine();
        _ansiConsole.WriteLine("Suggestions:");

        var findCommandGroupNodeResult = _commandStore.FindCommandGroupNode(args);

        if (findCommandGroupNodeResult?.Node is null)
        {
            _ansiConsole.WriteLine("No matches found");

            return;
        }

        _commandHelpHandler.PrintHelpFor(findCommandGroupNodeResult.Node.ChildNodes);
    }
}
