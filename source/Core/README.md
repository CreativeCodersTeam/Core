# CreativeCoders.Core

[![NuGet](https://img.shields.io/nuget/v/CreativeCoders.Core?style=flat-square)](https://www.nuget.org/packages/CreativeCoders.Core)
[![Build](https://img.shields.io/github/actions/workflow/status/CreativeCodersTeam/Core/build.yml?style=flat-square)](https://github.com/CreativeCodersTeam/Core/actions)
[![License](https://img.shields.io/github/license/CreativeCodersTeam/Core?style=flat-square)](../../LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-512bd4?style=flat-square)](https://dotnet.microsoft.com)

A foundational .NET library providing essential utilities, abstractions, and helpers for building robust applications. It serves as the base layer of the CreativeCoders ecosystem, offering a broad set of reusable primitives with minimal external dependencies.

[Features](#features) • [Installation](#installation) • [Usage](#usage) • [API Overview](#api-overview)

## Features

- **Argument validation** — fluent, chainable `Ensure` API with `CallerArgumentExpression` support
- **Caching** — generic cache interface with absolute/sliding expiration policies
- **Collections** — thread-safe lists, observable collections, batch operations, and LINQ extensions
- **Threading** — `ReaderWriterLockSlim`-backed primitives, synchronized values, and mutex locks
- **Enum utilities** — string ↔ enum conversion with custom attributes, reflection helpers
- **Dependency sorting** — topological sort with circular reference detection
- **Object linking** — declarative one-way / two-way property synchronization between objects
- **Visitor pattern** — generic visitor and visitable interfaces
- **Weak references** — `WeakAction`, `WeakFunc` and weak delegate wrappers
- **Reflection** — expression-based member name extraction, type and method utilities
- **I/O abstractions** — testable wrappers around `System.IO` via `System.IO.Abstractions`
- **Environment abstraction** — mockable `IEnvironment` / `Env` for unit testing
- **Text utilities** — string extensions, `SecureString` conversion, pattern matching

## Installation

```bash
dotnet add package CreativeCoders.Core
```

## Usage

### Argument validation

```csharp
public void ProcessText(string text, int maxLength)
{
    // Simple null/empty checks — argument name is inferred automatically
    Ensure.IsNotNullOrEmpty(text);
    Ensure.FileExists(fileName);

    // Chainable fluent checks
    Ensure.Argument(text).NotNull().HasMaxLength(maxLength);
}
```

### Caching

```csharp
ICache<string, UserData> cache = new DictionaryCache<string, UserData>();

var user = await cache.GetOrAddAsync("user-42", async key =>
    await _userRepository.FindAsync(key));
```

### Thread-safe collections

```csharp
var list = new ConcurrentList<string>(new LockSlimLockingMechanism());
list.Add("item");

var safeValue = new SynchronizedValue<int>(0);
safeValue.Value = 42;
```

### Enum ↔ string conversion

```csharp
enum Status
{
    [EnumStringValue("active")]   Active,
    [EnumStringValue("inactive")] Inactive
}

var converter = new EnumStringConverter();

string text   = converter.Convert(Status.Active);   // "active"
Status status = converter.Convert<Status>("active"); // Status.Active
```

### Dependency sorting

```csharp
// Sorts items in topological order; throws CircularReferenceException on cycles
var sorter = new DependencySorter<string>();
IEnumerable<string> sorted = sorter.Sort(items);
```

### Object property linking

```csharp
var link = new ObjectLinkBuilder()
    .From(source, s => s.Name)
    .To(target, t => t.DisplayName)
    .TwoWay()
    .Build();
```

### Environment abstraction

```csharp
// Register in DI
services.AddEnvironment();

// Inject and use — easily mockable in unit tests
public class MyService(IEnvironment env)
{
    public void PrintInfo()
    {
        Console.WriteLine($"Machine: {env.MachineName}");
        Console.WriteLine($"User:    {env.UserName}");
        Console.WriteLine($"Dir:     {env.CurrentDirectory}");
    }
}

// Or use the static wrapper directly
string desktop = Env.GetFolderPath(Environment.SpecialFolder.Desktop);
```

### Observable object base

```csharp
public class PersonViewModel : ObservableObject
{
    private string _name = string.Empty;

    public string Name
    {
        get => _name;
        set => Set(ref _name, value); // raises PropertyChanged automatically
    }
}
```

## API Overview

| Namespace | Highlights |
|---|---|
| `CreativeCoders.Core` | `Ensure`, `ObservableObject`, `EventHandlerEx`, object/disposable extensions |
| `CreativeCoders.Core.Caching` | `ICache<TKey,TValue>`, `ICacheExpirationPolicy`, `CacheExpirationMode` |
| `CreativeCoders.Core.Collections` | `ConcurrentList<T>`, `ExtendedObservableCollection<T>`, enumerable extensions |
| `CreativeCoders.Core.Comparing` | `FuncComparer<T>`, `MultiComparer<T>`, `FuncEqualityComparer<T>` |
| `CreativeCoders.Core.Dependencies` | `DependencySorter`, `DependencyTreeBuilder`, `CircularReferenceException` |
| `CreativeCoders.Core.Enums` | `EnumStringConverter`, `[EnumStringValue]`, `EnumExtensions` |
| `CreativeCoders.Core.Executing` | `IExecutable`, `IExecutableWithParameter<T>`, `IExecutableWithResult` |
| `CreativeCoders.Core.IO` | `FileSys`, `FileSystemEx`, `PathExtensions`, `StreamExtensions` |
| `CreativeCoders.Core.Messaging` | Event messaging infrastructure and message types |
| `CreativeCoders.Core.ObjectLinking` | `ObjectLink`, `ObjectLinkBuilder`, `[PropertyLink]` attribute |
| `CreativeCoders.Core.Reflection` | `ExpressionExtensions`, `TypeExtensions`, `ReflectionUtils` |
| `CreativeCoders.Core.SysEnvironment` | `IEnvironment`, `Env`, `EnvironmentWrapper`, DI extensions |
| `CreativeCoders.Core.Text` | `StringExtension`, `TextSpan`, `RandomString`, `PatternMatcher` |
| `CreativeCoders.Core.Threading` | `SynchronizedValue<T>`, locking mechanisms, `MutexLock` |
| `CreativeCoders.Core.Visitors` | `IVisitor<,>`, `Visitable`, `ListVisitor`, `VisitableAction` |
| `CreativeCoders.Core.Weak` | `WeakAction`, `WeakFunc<T>`, `WeakActionGeneric<T>` |

> [!NOTE]
> The library targets **.NET 10.0** and uses C# 14 features throughout, including file-scoped namespaces and nullable reference types.

## Dependencies

| Package | Purpose |
|---|---|
| `JetBrains.Annotations` | Nullability and contract annotations (`[PublicAPI]`, `[ContractAnnotation]`) |
| `Microsoft.Extensions.DependencyInjection.Abstractions` | DI integration for `IServiceCollection` extensions |
| `System.IO.Abstractions` | Testable file system abstraction |
