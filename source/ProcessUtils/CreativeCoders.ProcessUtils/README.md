# CreativeCoders.ProcessUtils

[![NuGet](https://img.shields.io/nuget/v/CreativeCoders.ProcessUtils?style=flat-square)](https://www.nuget.org/packages/CreativeCoders.ProcessUtils)

A small .NET library for running external processes from C# code. It wraps
`System.Diagnostics.Process` behind a testable abstraction and adds a fluent
executor with sync/async APIs, argument placeholders, output parsing and
opt-in error handling.

## Installation

```bash
dotnet add package CreativeCoders.ProcessUtils
```

## Features

- **Testable process abstraction** — `IProcess` / `IProcessFactory` wrap `System.Diagnostics.Process`
- **Fluent executor builder** — configure file name, arguments, `ProcessStartInfo` and error behavior in one chain
- **Sync and async execution** — `Execute`, `ExecuteAsync`, `ExecuteAndReturnExitCode(Async)`
- **Typed output parsing** — `IProcessOutputParser<T>` with `PassThrough`, `SplitLines` and `Json<T>` parsers
- **Argument placeholders** — substitute `{{name}}` tokens from a dictionary or an object's properties
- **Throw-on-error mode** — surface non-zero exit codes as `ProcessExecutionFailedException`
- **DI integration** — single `AddProcessUtils()` registration for `IServiceCollection`

## Usage

### Register the services

```csharp
using CreativeCoders.ProcessUtils;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddProcessUtils();
```

This registers:

- `IProcessFactory` → `DefaultProcessFactory` (singleton)
- `IProcessExecutorBuilder` and `IProcessExecutorBuilder<T>` (transient)

### Start a process directly

For low-level scenarios, inject `IProcessFactory`:

```csharp
public class GitCaller(IProcessFactory processFactory)
{
    public void ShowStatus()
    {
        using var process = processFactory.StartProcess("git", ["status"]);
        process.WaitForExit();
    }
}
```

### Execute a process and get the exit code

`IProcessExecutorBuilder` provides a non-generic, fluent builder for fire-and-forget
execution:

```csharp
public class Pinger(IProcessExecutorBuilder builder)
{
    public int Ping(string host)
    {
        var executor = builder
            .SetFileName("ping")
            .SetArguments(["-c", "1", host])
            .Build();

        return executor.ExecuteAndReturnExitCode();
    }
}
```

Async variants are available for every execution method:

```csharp
var exitCode = await executor.ExecuteAndReturnExitCodeAsync();
```

### Capture and parse output

`IProcessExecutorBuilder<T>` adds typed output parsing. Several parsers are
shipped in `CreativeCoders.ProcessUtils.Execution.Parsers`:

| Parser | Output type | Description |
| --- | --- | --- |
| `PassThroughProcessOutputParser` | `string` | Returns standard output as-is (used automatically when `T` is `string`) |
| `SplitLinesOutputParser` | `string[]` | Splits standard output by line, with optional trimming and `StringSplitOptions` |
| `JsonOutputParser<T>` | `T` | Deserializes standard output as JSON via `System.Text.Json` |

```csharp
public class DotnetSdks(IProcessExecutorBuilder<string[]> builder)
{
    public string[] List()
    {
        var executor = builder
            .SetFileName("dotnet")
            .SetArguments(["--list-sdks"])
            .SetOutputParser<SplitLinesOutputParser>(p => p.TrimLines = true)
            .Build();

        return executor.Execute() ?? [];
    }
}
```

Returning a plain `string` does not require an explicit parser:

```csharp
var output = builder
    .SetFileName("uname")
    .SetArguments(["-a"])
    .Build()
    .Execute();
```

For JSON output:

```csharp
var executor = builder
    .SetFileName("kubectl")
    .SetArguments(["get", "pods", "-o", "json"])
    .SetOutputParser<JsonOutputParser<PodList>>()
    .Build();

var pods = await executor.ExecuteAsync();
```

### Argument placeholders

Arguments can contain `{{name}}` placeholders that are replaced at execution
time. Pass either an `IDictionary<string, object?>` or any object whose
properties match the placeholder names:

```csharp
var executor = builder
    .SetFileName("git")
    .SetArguments(["clone", "{{url}}", "{{target}}"])
    .Build();

executor.Execute(new Dictionary<string, object?>
{
    ["url"] = "https://github.com/CreativeCodersTeam/Core.git",
    ["target"] = "./Core"
});

executor.Execute(new { url = "https://...", target = "./Core" });
```

### Error handling

By default the executor returns whatever exit code the process produced. Enable
`ShouldThrowOnError` to turn non-zero exit codes into a
`ProcessExecutionFailedException` that carries the captured `stderr`:

```csharp
try
{
    builder
        .SetFileName("dotnet")
        .SetArguments(["build", "Missing.sln"])
        .ShouldThrowOnError()
        .Build()
        .Execute();
}
catch (ProcessExecutionFailedException ex)
{
    Console.WriteLine($"Exit {ex.ExitCode}: {ex.ErrorOutput}");
}
```

### Customizing `ProcessStartInfo`

For advanced scenarios (working directory, environment variables, redirection
flags) use `SetupStartInfo`:

```csharp
builder
    .SetFileName("npm")
    .SetArguments(["install"])
    .SetupStartInfo(info =>
    {
        info.WorkingDirectory = "/repo";
        info.Environment["CI"] = "true";
    })
    .Build()
    .Execute();
```

### Accessing the underlying `IProcess`

The `ExecuteEx` family returns the underlying `IProcess` (non-generic) or a
`ProcessExecutionResult<T>` (generic) so callers can inspect exit codes,
process metadata, or read raw streams. Both results are `IDisposable`:

```csharp
using var result = builder
    .SetFileName("git")
    .SetArguments(["rev-parse", "HEAD"])
    .Build()
    .ExecuteEx();

Console.WriteLine($"git exited with {result.ExitCode}");
```

## API Overview

| Type | Purpose |
| --- | --- |
| `IProcess` / `DefaultProcess` | Testable wrapper over `System.Diagnostics.Process` |
| `IProcessFactory` / `DefaultProcessFactory` | Creates and starts `IProcess` instances |
| `IProcessExecutor` / `IProcessExecutor<T>` | High-level sync/async execution APIs |
| `IProcessExecutorBuilder` / `IProcessExecutorBuilder<T>` | Fluent builders for executors |
| `IProcessOutputParser<T>` | Strategy for converting standard output into `T` |
| `ProcessExecutionResult<T>` | Disposable result containing the parsed value and the `IProcess` |
| `ProcessExecutionFailedException` | Raised when `ShouldThrowOnError` is enabled and the process exits non-zero |
| `ProcessUtilsServiceCollectionExtensions.AddProcessUtils` | Registers the package's services in DI |
