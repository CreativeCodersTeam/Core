# CreativeCoders.Cli

A lightweight, convention-based CLI hosting framework for .NET that lets you build command-line applications with structured commands, automatic help generation, and dependency injection — all with minimal boilerplate.

## Packages

| Package | Description |
|---|---|
| `CreativeCoders.Cli.Core` | Core abstractions: command interfaces, attributes, and result types |
| `CreativeCoders.Cli.Hosting` | Hosting infrastructure: builder, command discovery, help, pre/post-processors |

## Getting started

### Installation

```bash
dotnet add package CreativeCoders.Cli.Core
dotnet add package CreativeCoders.Cli.Hosting
```

### Minimal example

Create a CLI host in your `Program.cs`:

```csharp
using CreativeCoders.Cli.Hosting;
using CreativeCoders.Cli.Hosting.Help;

await CliHostBuilder.Create()
    .EnableHelp(HelpCommandKind.CommandOrArgument)
    .Build()
    .RunMainAsync(args);
```

Define a command:

```csharp
using CreativeCoders.Cli.Core;

[CliCommand(["greet"], Name = "greet", Description = "Prints a greeting")]
public class GreetCommand : ICliCommand
{
    public Task<CommandResult> ExecuteAsync()
    {
        Console.WriteLine("Hello from the CLI!");
        return Task.FromResult(CommandResult.Success);
    }
}
```

Run it:

```bash
dotnet run -- greet
# Output: Hello from the CLI!

dotnet run -- --help
# Output: Lists all available commands
```

The hosting framework automatically discovers commands in the entry assembly by scanning for classes decorated with `[CliCommand]`.

## Commands with options

Commands can accept strongly-typed options that are automatically parsed from arguments:

```csharp
using CreativeCoders.SysConsole.Cli.Parsing;

public class DeployOptions
{
    [OptionValue(0, HelpText = "The target environment")]
    public string? Environment { get; set; }

    [OptionParameter('t', "tag", HelpText = "Docker image tag", DefaultValue = "latest")]
    public string? Tag { get; set; }

    [OptionParameter('d', "dry-run", HelpText = "Preview without applying changes")]
    public bool DryRun { get; set; }
}
```

```csharp
[CliCommand(["deploy"], Name = "deploy", Description = "Deploy the application")]
public class DeployCommand : ICliCommand<DeployOptions>
{
    public Task<CommandResult> ExecuteAsync(DeployOptions options)
    {
        Console.WriteLine($"Deploying to {options.Environment} with tag {options.Tag}");

        if (options.DryRun)
        {
            Console.WriteLine("(dry run — no changes applied)");
        }

        return Task.FromResult(CommandResult.Success);
    }
}
```

```bash
dotnet run -- deploy staging --tag v2.1.0 --dry-run
```

## Command groups

Organize related commands under a common group using multi-segment command paths:

```csharp
using CreativeCoders.Cli.Core;

// Register the group (assembly-level attribute)
[assembly: CliCommandGroup(["project"], "Project management commands")]
```

```csharp
[CliCommand(["project", "init"], Name = "init", Description = "Initialize a new project")]
public class ProjectInitCommand : ICliCommand
{
    public Task<CommandResult> ExecuteAsync()
    {
        Console.WriteLine("Project initialized.");
        return Task.FromResult(CommandResult.Success);
    }
}

[CliCommand(["project", "build"], Name = "build", Description = "Build the project")]
public class ProjectBuildCommand : ICliCommand
{
    public Task<CommandResult> ExecuteAsync()
    {
        Console.WriteLine("Project built.");
        return Task.FromResult(CommandResult.Success);
    }
}
```

```bash
dotnet run -- project init
dotnet run -- project build
dotnet run -- help project    # Lists commands in the "project" group
```

## Alternative commands

Commands can define aliases via `AlternativeCommands`:

```csharp
[CliCommand(["project", "init"], AlternativeCommands = ["init"])]
public class ProjectInitCommand : ICliCommand { /* ... */ }
```

Now both `project init` and `init` invoke the same command.

## Dependency injection

Commands are resolved through the DI container. Register services on the builder:

```csharp
var host = CliHostBuilder.Create()
    .EnableHelp(HelpCommandKind.CommandOrArgument)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IDeployService, DeployService>();
    })
    .Build();

await host.RunMainAsync(args);
```

Inject dependencies via primary constructors:

```csharp
[CliCommand(["deploy"], Name = "deploy", Description = "Deploy the application")]
public class DeployCommand(IDeployService deployService) : ICliCommand
{
    public async Task<CommandResult> ExecuteAsync()
    {
        await deployService.DeployAsync();
        return CommandResult.Success;
    }
}
```

## Options validation

Enable validation on the host builder and implement `IOptionsValidation` on your options class:

```csharp
var host = CliHostBuilder.Create()
    .UseValidation()
    .EnableHelp(HelpCommandKind.CommandOrArgument)
    .Build();
```

```csharp
public class DeployOptions : IOptionsValidation
{
    [OptionValue(0, HelpText = "The target environment")]
    public string? Environment { get; set; }

    public Task<OptionsValidationResult> ValidateAsync()
    {
        if (string.IsNullOrWhiteSpace(Environment))
        {
            return Task.FromResult(
                OptionsValidationResult.Invalid(["Environment is required"]));
        }

        return Task.FromResult(OptionsValidationResult.Valid());
    }
}
```

When validation fails, the error messages are printed to the console automatically.

## Pre- and post-processors

Add logic that runs before or after every command — useful for headers, footers, or telemetry:

```csharp
var host = CliHostBuilder.Create()
    .EnableHelp(HelpCommandKind.CommandOrArgument)
    .PrintHeaderMarkup(["[bold green]My CLI Tool v1.0[/]"])
    .PrintFooterText(["Done."])
    .Build();
```

For custom logic, implement `ICliPreProcessor` or `ICliPostProcessor`:

```csharp
public class TimingPreProcessor : ICliPreProcessor
{
    public CliProcessorExecutionCondition ExecutionCondition
        => CliProcessorExecutionCondition.OnlyOnCommand;

    public Task ExecuteAsync(string[] args)
    {
        Console.WriteLine($"Started at {DateTime.Now:T}");
        return Task.CompletedTask;
    }
}
```

Register it on the builder:

```csharp
builder.RegisterPreProcessor<TimingPreProcessor>();
```

The `CliProcessorExecutionCondition` controls when the processor runs:

| Value | Runs when |
|---|---|
| `Always` | Every CLI invocation |
| `OnlyOnCommand` | A command is executed |
| `OnlyOnHelp` | Help output is displayed |

## Help system

Enable help with one or more `HelpCommandKind` values:

```csharp
builder.EnableHelp(HelpCommandKind.CommandOrArgument);
```

| Kind | Trigger |
|---|---|
| `Command` | `help` as the first argument |
| `Argument` | `--help` anywhere in args |
| `EmptyArgs` | No arguments provided |
| `CommandOrArgument` | Either `help` or `--help` |

Help output is auto-generated from command attributes and option annotations.

## Configuration

Add configuration sources (JSON files, environment variables, etc.) via `UseConfiguration`:

```csharp
builder.UseConfiguration(config =>
{
    config.AddJsonFile("appsettings.json", optional: true);
    config.AddEnvironmentVariables();
});
```

The resulting `IConfiguration` is available for injection in your commands.

## Assembly scanning

By default, the entry assembly is scanned for commands. To scan additional assemblies:

```csharp
builder.ScanAssemblies(typeof(SomeCommandInAnotherAssembly).Assembly);
```

To disable entry assembly scanning (e.g., when all commands live in external libraries):

```csharp
builder.SkipScanEntryAssembly();
```

## Full example

A complete sample application is available in [`samples/CliHostSampleApp`](../../samples/CliHostSampleApp).
