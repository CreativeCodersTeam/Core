# CLI Command Base Classes — Design

**Date:** 2026-04-27
**Status:** Draft
**Target package:** `CreativeCoders.Cli.Commands` (new)
**Targets:** `CreativeCoders.Cli.Core.ICliCommand<TOptions>`

## Goal

Provide a small, well-bounded set of reusable base classes for `ICliCommand<TOptions>` that cover the most common CLI command needs without forcing consumers to inherit unwanted behavior. Keep dependencies optional by isolating them in a separate package.

Scope (chosen during brainstorming):

- **A** — Cross-cutting concerns (dry-run, confirmation, verbosity, exception handling)
- **B** — Output formatting (JSON / Table / Plain)
- **C** — Interaction (Spectre.Console-based prompts, optional auto-prompt for missing options)
- **D** — CRUD command hierarchy

Explicitly **out of scope**: long-running/progress (E), batch/pipeline (F), config/state (G).

## Design Decisions

| Decision | Choice | Rationale |
|---|---|---|
| Target API | `Cli.Core.ICliCommand<TOptions>` only | The newer hosting-based CLI; `SysConsole.CliArguments` is legacy and not in scope |
| Distribution | New package `CreativeCoders.Cli.Commands` | Keeps Spectre/JSON dependencies out of `Cli.Core`; opt-in for consumers |
| Composition for A/B/C | Single base class + marker interfaces on options + template-method hooks | Avoids the combinatorial explosion of deep inheritance hierarchies. Each concern is opt-in by implementing the matching marker interface. |
| CRUD layering | Abstract base **and** repository-driven base | Two layers: abstract method for max flexibility, plus repository variant on top for the common case |

## Architecture

### Package structure

```
CreativeCoders.Cli.Commands/
  CliCommandBase.cs                 // CliCommandBase<TOptions>, CliCommandBase<TOptions, TResult>
  ExitCodes.cs                      // Cancelled = 130, NotFound = 2, Error = 1
  Options/
    IDryRunOptions.cs
    IConfirmableOptions.cs
    IVerbosityOptions.cs
    IOutputFormatOptions.cs
    IInteractiveOptions.cs
    Verbosity.cs                    // enum: Quiet | Normal | Verbose
    OutputFormat.cs                 // enum: Json | Table | Plain (extensible by string id later if needed)
  Confirmation/
    IConfirmationPrompt.cs
    SpectreConfirmationPrompt.cs    // default impl
  Output/
    IOutputFormatter.cs             // IOutputFormatter<TResult>
    JsonOutputFormatter.cs
    TableOutputFormatter.cs
    PlainOutputFormatter.cs
  Interaction/
    IInteractivePrompter.cs
    SpectreInteractivePrompter.cs
  Crud/
    ICrudRepository.cs
    IKeyedOptions.cs
    IEntityInputOptions.cs
    ListCommandBase.cs
    GetCommandBase.cs
    AddCommandBase.cs
    UpdateCommandBase.cs
    RemoveCommandBase.cs
    RepositoryListCommandBase.cs
    RepositoryGetCommandBase.cs
    RepositoryAddCommandBase.cs
    RepositoryUpdateCommandBase.cs
    RepositoryRemoveCommandBase.cs
    Convenience/
      ListOptions.cs                // pre-built record with IOutputFormatOptions + IVerbosityOptions
      RemoveOptions.cs              // pre-built with IKeyedOptions<TKey> + IConfirmableOptions + IDryRunOptions
      AddOptions.cs
      UpdateOptions.cs
  ServiceCollectionExtensions.cs    // AddCliCommandsBaseClasses()
```

Dependencies: `CreativeCoders.Cli.Core`, `Spectre.Console`, `System.Text.Json` (already in BCL).

### `CliCommandBase<TOptions>` template flow

`ExecuteAsync(TOptions options)` runs this sequence:

```
1. Resolve cancellation token (from host or self-managed Console.CancelKeyPress)
2. If TOptions : IVerbosityOptions  → apply verbosity (wraps IAnsiConsole / logging filter)
3. If TOptions : IInteractiveOptions and TTY and !NoInteractive
       → call PromptMissingOptionsAsync(options, prompter, ct)
4. If TOptions : IConfirmableOptions and !options.Yes and RequiresConfirmation(options)
       → call IConfirmationPrompt.ConfirmAsync(GetConfirmationMessage(options))
       → if no  → return CommandResult(ExitCodes.Cancelled)
5. If TOptions : IDryRunOptions and options.DryRun
       → call OnDryRunAsync(options, ct); return CommandResult.Success
   else
       → call OnExecuteAsync(options, ct)
6. (Result variant only) if TOptions : IOutputFormatOptions
       → resolve IOutputFormatter<TResult> for options.Format from DI; write result
7. Wrap entire flow in try/catch:
       OperationCanceledException → CommandResult(ExitCodes.Cancelled)
       any other Exception        → OnHandleExceptionAsync(ex, options)
```

### Hooks (all `protected virtual` unless noted)

```csharp
// CliCommandBase<TOptions>
protected abstract Task<CommandResult> OnExecuteAsync(TOptions options, CancellationToken ct);
protected virtual Task OnDryRunAsync(TOptions options, CancellationToken ct);
protected virtual string GetConfirmationMessage(TOptions options);
protected virtual bool RequiresConfirmation(TOptions options) => true;
protected virtual Task<CommandResult> OnHandleExceptionAsync(Exception ex, TOptions options);
protected virtual Task PromptMissingOptionsAsync(
    TOptions options, IInteractivePrompter prompter, CancellationToken ct);

// CliCommandBase<TOptions, TResult>
protected abstract Task<TResult> OnExecuteAsync(TOptions options, CancellationToken ct);
protected virtual Task WriteResultAsync(TResult result, OutputFormat format, CancellationToken ct);
```

Helper methods on the base class (TTY-aware, throw `InvalidOperationException` when no TTY):

```csharp
protected Task<T> PromptAsync<T>(string message, T? defaultValue = default);
protected Task<bool> ConfirmAsync(string message, bool defaultValue = false);
protected Task<T> SelectAsync<T>(string message, IEnumerable<T> choices);
protected Task<IReadOnlyList<T>> MultiSelectAsync<T>(string message, IEnumerable<T> choices);
```

### Marker interfaces (cross-cutting)

```csharp
public interface IDryRunOptions       { bool DryRun     { get; set; } }
public interface IConfirmableOptions  { bool Yes        { get; set; } }
public interface IVerbosityOptions    { Verbosity Verbosity { get; set; } }
public interface IOutputFormatOptions { OutputFormat Format { get; set; } }
public interface IInteractiveOptions  { bool NoInteractive { get; set; } }
```

Argument parsing: the base class does **not** auto-bind `--dry-run`, `--yes`, `--verbosity`, etc. to options. That is the responsibility of the consumer's argument parser pipeline. The base class only inspects the parsed options object for the marker interfaces. Conventions are documented (recommended attribute usage), nothing enforced.

### Output formatting

```csharp
public interface IOutputFormatter<in TResult>
{
    OutputFormat Format { get; }
    Task WriteAsync(TResult value, IAnsiConsole console, CancellationToken ct);
}
```

Default formatters registered by `AddCliCommandsBaseClasses()`:

- `JsonOutputFormatter<TResult>` — `System.Text.Json` with configurable `JsonSerializerOptions` (default: camelCase, indented). Always available via open-generic registration.
- `TableOutputFormatter<TResult>` — Spectre.Console table. Default reflection-based for `IEnumerable<T>` (properties → columns). Consumer can register a specialized `IOutputFormatter<TResult>` to override.
- `PlainOutputFormatter<TResult>` — `ToString()` per line for sequences, direct otherwise.

Selection: from DI, take all `IOutputFormatter<TResult>` and pick the one whose `Format` matches `options.Format`. Fallback to `Plain` if none.

**TTY-awareness:** when `Console.IsOutputRedirected` is true and `Format` was not explicitly set by the user, default to `Plain` instead of `Table` so output stays pipe-safe (no ANSI codes).

### Interaction

Two distinct mechanisms:

1. **Auto-prompt for missing options** — gated by `IInteractiveOptions` and TTY. Default `PromptMissingOptionsAsync` is empty; consumers override and prompt for required fields that are still null/empty.
2. **Helper methods on the base class** — wrap `IAnsiConsole`. Available regardless of `IInteractiveOptions`. In non-TTY environments they throw `InvalidOperationException` with a clear message, preventing CI hangs.

A "wizard" command is just a `CliCommandBase<TOptions>` that uses the helpers in `OnExecuteAsync`. No dedicated `WizardCommandBase` — it would add no behavior over the base.

### CRUD hierarchy

#### Repository contract

```csharp
public interface ICrudRepository<TEntity, TKey>
{
    Task<IReadOnlyList<TEntity>> ListAsync(CancellationToken ct);
    Task<TEntity?> GetAsync(TKey key, CancellationToken ct);
    Task<TEntity>  AddAsync(TEntity entity, CancellationToken ct);
    Task<TEntity>  UpdateAsync(TKey key, TEntity entity, CancellationToken ct);
    Task           RemoveAsync(TKey key, CancellationToken ct);
}
```

Deliberately no `IQueryable`, no filtering, no pagination. YAGNI. Consumers needing more write their own base.

#### Marker interfaces for CRUD options

```csharp
public interface IKeyedOptions<TKey>      { TKey Key       { get; set; } }
public interface IEntityInputOptions<T>   { T   Entity     { get; set; } }   // optional; alternatively use BuildEntityAsync hook
```

#### Class hierarchy

| Operation | Abstract base | Repository-driven base |
|---|---|---|
| List   | `ListCommandBase<TEntity, TOptions>` → `LoadAsync(TOptions, ct)` | `RepositoryListCommandBase<TEntity, TKey, TOptions>` |
| Get    | `GetCommandBase<TEntity, TKey, TOptions>` (TOptions : IKeyedOptions<TKey>) → `LoadByKeyAsync(TKey, ct)` | `RepositoryGetCommandBase<…>` |
| Add    | `AddCommandBase<TEntity, TOptions>` → `BuildEntityAsync` + `PersistAsync` | `RepositoryAddCommandBase<…>` |
| Update | `UpdateCommandBase<TEntity, TKey, TOptions>` → `LoadByKeyAsync` + `BuildEntityAsync` + `PersistAsync` | `RepositoryUpdateCommandBase<…>` |
| Remove | `RemoveCommandBase<TEntity, TKey, TOptions>` → `RemoveByKeyAsync(TKey, ct)` | `RepositoryRemoveCommandBase<…>` |

All CRUD bases inherit `CliCommandBase<TOptions, TResult>` so output formatting works automatically when `TOptions : IOutputFormatOptions`.

#### Built-in defaults

- `RemoveCommandBase` defaults `RequiresConfirmation` to `true` and `GetConfirmationMessage` to `$"Really remove '{key}'?"`. Override or pass `--yes` to skip.
- `ListCommandBase` `TResult = IReadOnlyList<TEntity>`. The default `TableOutputFormatter` renders `IEnumerable` automatically.
- `GetCommandBase` `TResult = TEntity`. On `null` returns `CommandResult(ExitCodes.NotFound)` (default 2).
- `AddCommandBase` / `UpdateCommandBase` `TResult = TEntity` (the persisted entity), formatted on output.

#### Convenience option records (optional)

Pre-built records consumers may use directly or derive from:

- `ListOptions` — `IOutputFormatOptions, IVerbosityOptions`
- `RemoveOptions<TKey>` — `IKeyedOptions<TKey>, IConfirmableOptions, IDryRunOptions, IVerbosityOptions`
- `AddOptions<TEntity>` — `IEntityInputOptions<TEntity>, IDryRunOptions, IOutputFormatOptions, IVerbosityOptions`
- `UpdateOptions<TKey, TEntity>` — `IKeyedOptions<TKey>, IEntityInputOptions<TEntity>, IConfirmableOptions, IDryRunOptions, IOutputFormatOptions, IVerbosityOptions`

### DI registration

```csharp
public static IServiceCollection AddCliCommandsBaseClasses(this IServiceCollection services)
{
    Ensure.NotNull(services);

    services.TryAddSingleton<IConfirmationPrompt, SpectreConfirmationPrompt>();
    services.TryAddSingleton<IInteractivePrompter, SpectreInteractivePrompter>();

    // open-generic formatters — multiple registrations of the same service type,
    // so use AddTransient (TryAdd would skip the 2nd/3rd)
    services.AddTransient(typeof(IOutputFormatter<>), typeof(JsonOutputFormatter<>));
    services.AddTransient(typeof(IOutputFormatter<>), typeof(TableOutputFormatter<>));
    services.AddTransient(typeof(IOutputFormatter<>), typeof(PlainOutputFormatter<>));

    return services;
}
```

Consumers override defaults by registering more specific `IOutputFormatter<TConcreteResult>` implementations before/after.

### Cancellation

`CliCommandBase` requires a `CancellationToken` on every hook. If the host already passes one (preferred), use it. If not (current `Cli.Core.ICliCommand<TOptions>.ExecuteAsync(TOptions)` has no token parameter), the base creates its own `CancellationTokenSource` and wires `Console.CancelKeyPress` to it. **This is a known shortcoming of the host API** and may be addressed in a follow-up by adding an overload to `ICliCommand`.

## Worked example

```csharp
public class DeleteUserOptions : IDryRunOptions, IConfirmableOptions, IVerbosityOptions
{
    [Option('u', "user")] public string UserName { get; set; } = "";
    public bool DryRun { get; set; }
    public bool Yes { get; set; }
    public Verbosity Verbosity { get; set; }
}

public class DeleteUserCommand(IUserService users) : CliCommandBase<DeleteUserOptions>
{
    private readonly IUserService _users = Ensure.NotNull(users);

    protected override string GetConfirmationMessage(DeleteUserOptions o)
        => $"Really delete user '{o.UserName}'?";

    protected override async Task OnDryRunAsync(DeleteUserOptions o, CancellationToken ct)
        => await Console.Out.WriteLineAsync($"Would delete '{o.UserName}'").ConfigureAwait(false);

    protected override async Task<CommandResult> OnExecuteAsync(DeleteUserOptions o, CancellationToken ct)
    {
        await _users.DeleteAsync(o.UserName, ct).ConfigureAwait(false);

        return CommandResult.Success;
    }
}
```

```csharp
public class ListUsersOptions : ListOptions { }

public class ListUsersCommand(ICrudRepository<User, Guid> repo)
    : RepositoryListCommandBase<User, Guid, ListUsersOptions>(repo);
```

## Testing strategy

- **Unit tests** for `CliCommandBase` template flow: each marker interface combination, dry-run path, confirmation cancel path, exception handling. Mock `IConfirmationPrompt`, `IInteractivePrompter`, `IOutputFormatter<>` via FakeItEasy.
- **Unit tests** for each default formatter (Json/Table/Plain) — golden-output assertions.
- **Unit tests** for each CRUD base class via fakes for `ICrudRepository`.
- **Integration sample** in `samples/` showing end-to-end use.
- Follow project conventions: xUnit, FakeItEasy, AwesomeAssertions, no `.ConfigureAwait(false)` in tests.

## Out of scope (explicit non-goals)

- Long-running / progress bars / cancellation UX (option E)
- Batch / pipeline / parallel item processing (option F)
- Config file / persistent state commands (option G)
- Filtering, pagination, search on the CRUD repository contract
- Auto-binding of `--dry-run` / `--yes` / `--verbosity` flags to options (parser concern, not base class concern)
- Backporting any of this to `CreativeCoders.SysConsole.CliArguments`
