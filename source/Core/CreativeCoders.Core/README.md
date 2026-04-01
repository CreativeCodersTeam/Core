# CreativeCoders.Core

[![NuGet](https://img.shields.io/nuget/v/CreativeCoders.Core?style=flat-square)](https://www.nuget.org/packages/CreativeCoders.Core)

A foundational .NET library providing essential utilities, extensions, and abstractions for building robust C# applications. From parameter validation and thread-safe collections to messaging, caching, and reflection helpers — everything you need to reduce boilerplate and write cleaner code.

## Installation

```bash
dotnet add package CreativeCoders.Core
```

## Features

- **Parameter Validation** — Guard clauses with `Ensure` and fluent `Argument<T>` validation
- **Collection Extensions** — LINQ-style helpers like `ForEach`, `Distinct` by key, `WhereNotNull`, and more
- **Thread-Safe Collections** — `ConcurrentList<T>`, `SynchronizedValue<T>`, and configurable locking
- **Observable Collections** — `ExtendedObservableCollection<T>` with batch updates and UI synchronization
- **In-Memory Caching** — Dictionary-based cache with expiration policies and region support
- **Chain of Responsibility** — `HandlerChain` for building processing pipelines
- **Pub/Sub Messaging** — Loosely-coupled messenger with weak reference support
- **Reflection Utilities** — Type discovery, generic instance creation, expression helpers
- **String Utilities** — Case conversion, pattern matching, SecureString support, placeholder replacement
- **Fluent Interface Helpers** — Extension methods for building fluent APIs
- **Environment Abstraction** — Testable `Env` wrapper over `System.Environment`
- **Visitor Pattern** — Full visitor infrastructure with sub-item traversal

## Usage

### Ensure — Parameter Validation

The `Ensure` class provides guard clauses for method parameters with aggressive inlining for zero-overhead validation.

```csharp
using CreativeCoders.Core;

public class UserService(IUserRepository repository)
{
    // Guard constructor-injected dependencies
    private readonly IUserRepository _repository = Ensure.NotNull(repository);

    public User GetUser(string email, Guid tenantId)
    {
        // Guard method arguments
        Ensure.IsNotNullOrWhitespace(email);
        Ensure.GuidIsNotEmpty(tenantId);

        return _repository.FindByEmail(email, tenantId);
    }

    public void UpdateUsers(IEnumerable<User> users)
    {
        // Guard collections against null or empty
        Ensure.IsNotNullOrEmpty(users);

        // Assert arbitrary conditions
        Ensure.That(users.All(u => u.IsValid), "All users must be valid");

        // Validate index ranges
        Ensure.IndexIsInRange(0, users.Count());
    }

    public void LoadConfig(string configPath)
    {
        // Guard file system paths
        Ensure.FileExists(configPath);
        Ensure.DirectoryExists(Path.GetDirectoryName(configPath)!);
    }
}
```

### Fluent Argument Validation

For a more fluent approach to parameter validation, use `Ensure.Argument`:

```csharp
using CreativeCoders.Core;

public void ProcessOrder(string orderId, int quantity, string text, IEnumerable<string> items)
{
    // Chain multiple checks for one argument
    Ensure.Argument(orderId).IsNotNullOrWhitespace();
    Ensure.Argument(items).IsNotNullOrEmpty();
    Ensure.Argument(text).NotNull().HasMaxLength(100);
}
```

### Object Extensions

Utility extensions for safe casting, null-safe operations, and reflection-based property access.

```csharp
using CreativeCoders.Core;

object value = GetValue();

// Null-safe ToString
string text = value.ToStringSafe("default");

// Safe type casting with default fallback
int number = value.As<int>(42);

// Try-pattern casting
if (value.TryAs<string>(out var str))
{
    Console.WriteLine(str);
}

// Convert public properties to a dictionary
var user = new { Name = "Alice", Age = 30 };
Dictionary<string, object?> dict = user.ToDictionary();

// Reflection-based property access
var name = someObject.GetPropertyValue<string>("Name");
someObject.SetPropertyValue("Name", "Bob");

// Polymorphic async disposal
await someObject.TryDisposeAsync();
```

### DelegateDisposable

Create `IDisposable` and `IAsyncDisposable` instances from delegates — useful for cleanup callbacks and scope guards.

```csharp
using CreativeCoders.Core;

// Create a disposable from an action
var cleanup = new DelegateDisposable(() => Console.WriteLine("Cleaned up!"), onlyDisposeOnce: true);

using (cleanup)
{
    // Do work...
} // "Cleaned up!" is printed

// Async variant
var asyncCleanup = new DelegateAsyncDisposable(
    async () => await SaveStateAsync(),
    onlyDisposeOnce: true);

await using (asyncCleanup)
{
    // Do work...
}
```

### Fluent Interface Extensions

Build fluent APIs by chaining operations that return `this`.

```csharp
using CreativeCoders.Core;

var builder = new StringBuilder()
    .Fluent(sb => sb.Append("Hello"))
    .Fluent(sb => sb.Append(" World"))
    .FluentIf(includeExclamation, sb => sb.Append("!"));

// Works on any type — great for builder patterns
var config = new ConfigBuilder()
    .Fluent(c => c.SetTimeout(30))
    .FluentIf(isDevelopment, c => c.EnableLogging());
```

### ObservableObject — MVVM Base Class

A base class implementing `INotifyPropertyChanged` and `INotifyPropertyChanging` for MVVM scenarios.

```csharp
using CreativeCoders.Core;

public class PersonViewModel : ObservableObject
{
    private string _name;
    private int _age;

    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }

    public int Age
    {
        get => _age;
        set => Set(ref _age, value);
    }
}
```

### Collections — Enumerable Extensions

Powerful LINQ-style extensions for everyday collection operations.

```csharp
using CreativeCoders.Core.Collections;

var items = new List<string> { "alpha", "beta", "gamma", "delta" };

// ForEach with index
items.ForEach((item, index) => Console.WriteLine($"{index}: {item}"));

// Async iteration
await items.ForEachAsync(async item => await ProcessAsync(item));

// Side-effect piping (lazy — executes during enumeration)
var processed = items
    .Pipe(item => Log(item))
    .Where(item => item.Length > 4)
    .ToList();

// Take elements until a condition is met (inclusive)
var untilGamma = items.TakeUntil(x => x == "gamma"); // ["alpha", "beta", "gamma"]

// Take every Nth element
var everyOther = items.TakeEvery(2); // ["alpha", "gamma"]

// Filter nulls
var nonNull = mixedList.WhereNotNull();

// Check if exactly one element matches
bool single = items.IsSingle(x => x.StartsWith("a")); // true
```

**Distinct and duplicate detection by key:**

```csharp
var people = new List<Person> { /* ... */ };

// Distinct by single key
var uniqueByName = people.Distinct(p => p.Name);

// Distinct by multiple keys
var uniqueByNameAndAge = people.Distinct(p => p.Name, p => p.Age);

// Find duplicates
var duplicates = people.NotDistinct(p => p.Email);
```

**Multi-key sorting with direction control:**

```csharp
using CreativeCoders.Core.Comparing;

var sorted = people.Sort(
    new SortFieldInfo<Person, string>(p => p.LastName, SortOrder.Ascending),
    new SortFieldInfo<Person, int>(p => p.Age, SortOrder.Descending));
```

**Choose — filter and transform in one step:**

```csharp
// Choose combines Where + Select
var parsed = strings.Choose(s =>
    int.TryParse(s, out var n) ? (true, n) : (false, 0));
```

### Collections — List and Dictionary Extensions

```csharp
using CreativeCoders.Core.Collections;

// Add multiple items to any IList<T>
IList<string> list = new List<string>();
list.AddRange(new[] { "a", "b", "c" });

// Replace all items at once
list.SetItems(new[] { "x", "y", "z" });

// Reverse dictionary lookup
var dict = new Dictionary<string, int> { ["alice"] = 1, ["bob"] = 2 };
string key = dict.GetKeyByValue(2); // "bob"
```

### Collections — ExtendedObservableCollection

Thread-safe observable collection with batch update support and UI synchronization.

```csharp
using CreativeCoders.Core.Collections;

var collection = new ExtendedObservableCollection<string>();

// Batch updates — raises a single CollectionChanged event
using (collection.Update())
{
    collection.Add("item1");
    collection.Add("item2");
    collection.Add("item3");
}

// Add multiple items at once
collection.AddRange(new[] { "item4", "item5" });

// Move items with proper notifications
collection.Move(oldIndex: 0, newIndex: 2);
```

### Caching

In-memory dictionary-based cache with expiration policies and named regions.

```csharp
using CreativeCoders.Core.Caching;
using CreativeCoders.Core.Caching.Default;

// Create a cache
var cache = CacheManager.CreateCache<string, UserProfile>();

// Get or add with lazy factory
var profile = cache.GetOrAdd("user:123", () => LoadProfileFromDb("123"));

// Use named regions for logical separation
cache.AddOrUpdate("user:456", profile, regionName: "premium-users");

// Try-get pattern
if (cache.TryGet("user:123", out var cached))
{
    Console.WriteLine(cached.Name);
}

// Async operations
var asyncProfile = await cache.GetOrAddAsync("user:789",
    () => LoadProfileFromDb("789"));

// Clear a specific region
cache.Clear(regionName: "premium-users");

// Remove a single entry
cache.Remove("user:123");
```

### Chaining — Chain of Responsibility

Build processing pipelines where handlers can choose to handle or pass data along.

```csharp
using CreativeCoders.Core.Chaining;

// Define handlers using the delegate-based handler
var handlers = new IChainDataHandler<string, string>[]
{
    new ChainDataHandler<string, string>(input =>
        input.StartsWith("http")
            ? new HandleResult<string>(true, $"URL: {input}")
            : new HandleResult<string>(false, default!)),

    new ChainDataHandler<string, string>(input =>
        input.Contains("@")
            ? new HandleResult<string>(true, $"Email: {input}")
            : new HandleResult<string>(false, default!)),

    new ChainDataHandler<string, string>(input =>
        new HandleResult<string>(true, $"Text: {input}"))
};

var chain = new HandlerChain<string, string>(handlers);

var result1 = chain.Handle("http://example.com"); // "URL: http://example.com"
var result2 = chain.Handle("user@mail.com");       // "Email: user@mail.com"
var result3 = chain.Handle("hello world");          // "Text: hello world"
```

### Comparing — Custom Comparers

Function-based comparers for use with LINQ, sorted collections, and deduplication.

```csharp
using CreativeCoders.Core.Comparing;

// Equality by key selector
var comparer = new FuncEqualityComparer<Person, string>(p => p.Email);
var unique = people.Distinct(comparer);

// Sorting by key with direction
var sorter = new FuncComparer<Person, string>(p => p.LastName, SortOrder.Ascending);
var sorted = people.Order(sorter);

// Combine multiple comparers — all must agree for equality
var strict = new MultiEqualityComparer<Person>(
    new FuncEqualityComparer<Person, string>(p => p.Name),
    new FuncEqualityComparer<Person, int>(p => p.Age));
```

### Enums — String Conversion and Flag Enumeration

```csharp
using CreativeCoders.Core.Enums;

public enum Status
{
    [EnumStringValue("not-started")]
    NotStarted,

    [EnumStringValue("in-progress")]
    InProgress,

    [EnumStringValue("done")]
    Done
}

// Convert enum to its string representation
string text = Status.InProgress.ToText(); // "in-progress"

// Enumerate individual flags from a flags enum
[Flags]
public enum Permissions { Read = 1, Write = 2, Execute = 4 }

var flags = (Permissions.Read | Permissions.Write);
var individual = flags.EnumerateFlags(); // [Read, Write]
```

### Messaging — Pub/Sub Messenger

A lightweight messenger for loosely-coupled communication between components, with weak reference support to prevent memory leaks.

```csharp
using CreativeCoders.Core.Messaging;

// Use the default singleton messenger
var messenger = Messenger.Default;

// Or create an isolated instance
var isolated = Messenger.CreateInstance();

// Register a message handler (returns IDisposable for unregistration)
var registration = messenger.Register<OrderPlacedMessage>(this, message =>
{
    Console.WriteLine($"Order {message.OrderId} placed!");
});

// Send a message to all registered handlers
messenger.Send(new OrderPlacedMessage { OrderId = "ORD-001" });

// Unregister via IDisposable
registration.Dispose();

// Or unregister all handlers for a receiver
messenger.Unregister(this);
```

### Placeholders — Template String Replacement

Replace named placeholders in text with configured values.

```csharp
using CreativeCoders.Core.Placeholders;

var placeholders = new Dictionary<string, object?>
{
    ["Name"] = "Alice",
    ["Role"] = "Admin",
    ["Date"] = DateTime.Now
};

var replacer = new PlaceholderReplacer("{", "}", placeholders);

string result = replacer.Replace("Hello {Name}, you are a {Role} since {Date}.");
// "Hello Alice, you are a Admin since 4/1/2026 ..."

// Replace in multiple lines at once
var lines = new[] { "User: {Name}", "Role: {Role}" };
var replaced = replacer.Replace(lines);
```

### Reflection — Type Discovery and Utilities

```csharp
using CreativeCoders.Core.Reflection;

// Find all implementations of an interface across loaded assemblies
IEnumerable<Type> handlers = typeof(ICommandHandler).GetImplementations();

// Create a generic type instance dynamically
object? cache = typeof(Cache<>).CreateGenericInstance(typeof(string));

// Get default value for any type
object? defaultVal = typeof(int).GetDefault(); // 0

// Check if a type implements a generic interface
bool isEnumerable = typeof(List<int>).ImplementsGenericInterface(typeof(IEnumerable<>));

// Get generic interface type arguments
Type[] args = typeof(List<int>).GetGenericInterfaceArguments(typeof(IEnumerable<>)); // [int]

// Extract member names from expressions (useful for MVVM/reflection)
string name = ExpressionExtensions.GetMemberName<Person>(p => p.Name); // "Name"
```

### Text — String Extensions

Case conversion, pattern matching, filtering, and more.

```csharp
using CreativeCoders.Core.Text;

// Null-safe string checks
string? text = GetText();
if (text.IsNotNullOrWhiteSpace())
{
    Console.WriteLine(text);
}

// Case conversions
"myProperty".CamelCaseToPascalCase();    // "MyProperty"
"my-component".KebabCaseToPascalCase();  // "MyComponent"
"my_field".SnakeCaseToPascalCase();      // "MyField"

// Character filtering
"Hello, World!".Filter(char.IsLetter);     // "HelloWorld"
"user@email.com".Filter('@', '.');         // "useremailcom"

// Split into key-value pair
var kv = "Content-Type=application/json".SplitIntoKeyValue("=");
// kv.Key == "Content-Type", kv.Value == "application/json"

// Conditional StringBuilder extensions
var sb = new StringBuilder();
sb.AppendLineIf(includeHeader, "=== Report ===");
sb.AppendIf(showTimestamp, $"[{DateTime.Now}] ");
```

**Pattern matching with wildcards:**

```csharp
using CreativeCoders.Core.Text;

bool match1 = PatternMatcher.MatchesPattern("report.pdf", "*.pdf");     // true
bool match2 = PatternMatcher.MatchesPattern("file01.txt", "file??.txt"); // true
bool match3 = PatternMatcher.MatchesPattern("data.csv", "*.json");      // false
```

**Random string generation:**

```csharp
using CreativeCoders.Core.Text;

string token = RandomString.Create();      // 128-byte random Base64 string
string short_ = RandomString.Create(32);   // 32-byte random Base64 string
```

### Text — JSON Serialization Abstraction

```csharp
using CreativeCoders.Core.Text.Json;

IJsonSerializer serializer = new DefaultJsonSerializer();

// Serialize
string json = serializer.Serialize(new { Name = "Alice", Age = 30 });

// Deserialize
var person = serializer.Deserialize<Person>(json);

// Populate an existing object (merge properties)
var existing = new Person { Name = "Bob" };
serializer.Populate("{\"Age\": 25}", existing);

// Async stream operations
await using var stream = File.OpenRead("data.json");
var data = await serializer.DeserializeAsync<Config>(stream);
```

### Threading — SynchronizedValue

Thread-safe wrapper for values with configurable locking strategies.

```csharp
using CreativeCoders.Core.Threading;

// Simple synchronized value
var counter = SynchronizedValue.Create(0);

// Thread-safe read and write
counter.Value = 42;
int current = counter.Value;

// Atomic update
counter.SetValue(c => c + 1);

// With custom locking mechanism
var lockMechanism = new LockSlimLockingMechanism();
var sharedState = SynchronizedValue.Create(lockMechanism, "initial");
```

### Threading — ConcurrentList

A thread-safe `IList<T>` implementation with configurable locking.

```csharp
using CreativeCoders.Core.Threading;

var list = new ConcurrentList<string>();

// All operations are thread-safe
list.Add("item1");
list.Add("item2");
bool contains = list.Contains("item1");
int count = list.Count;

// Initialize from existing collection
var fromItems = new ConcurrentList<int>(new[] { 1, 2, 3 });

// Use custom locking (e.g., no-lock for single-threaded scenarios)
var noLockList = new ConcurrentList<string>(new NoLockingMechanism());
```

### Threading — Locking Mechanisms

Pluggable locking strategies for thread synchronization.

```csharp
using CreativeCoders.Core.Threading;

// ReaderWriterLockSlim-based (default, best for read-heavy workloads)
ILockingMechanism rwLock = new LockSlimLockingMechanism();

rwLock.Read(() =>
{
    // Read-locked section — multiple readers allowed
    return cache.Get("key");
});

rwLock.Write(() =>
{
    // Write-locked section — exclusive access
    cache.Set("key", "value");
});

// Simple lock(object)-based
ILockingMechanism simpleLock = new LockLockingMechanism();

// No synchronization (for single-threaded code or testing)
ILockingMechanism noLock = new NoLockingMechanism();
```

### IO — File System Utilities

Extensions for `System.IO.Abstractions` with path safety checks and directory helpers.

```csharp
using CreativeCoders.Core.IO;

// Ensure directories exist
FileSys.Directory.EnsureDirectoryExists("/data/exports");
FileSys.Directory.EnsureDirectoryForFileNameExists("/data/exports/report.csv");

// Sanitize file names
string safe = FileSys.Path.ReplaceInvalidFileNameChars("file:name?.txt", "_");
// "file_name_.txt"

// Path safety — prevent directory traversal attacks
bool isSafe = FileSys.Path.IsSafe("../../../etc/passwd", "/data/uploads"); // false
FileSys.Path.EnsureSafe(userProvidedPath, allowedBasePath); // throws if unsafe
```

### SysEnvironment — Testable Environment Abstraction

A drop-in replacement for `System.Environment` that can be swapped for testing.

```csharp
using CreativeCoders.Core.SysEnvironment;

// Use like System.Environment
string user = Env.UserName;
string machine = Env.MachineName;
string? home = Env.GetEnvironmentVariable("HOME");

// Inject a mock for testing
using (Env.SetEnvironmentImpl(mockEnvironment))
{
    // All Env calls now go through mockEnvironment
    var testUser = Env.UserName; // returns mock value
} // Original environment is automatically restored

// Or use IEnvironment via DI
services.AddEnvironment();

public class MyApp(IEnvironment environment)
{
    public void Run()
    {
        Console.WriteLine($"User: {environment.UserName}");
        Console.WriteLine($"Machine: {environment.MachineName}");
    }
}
```

### Weak — Weak Reference Delegates

Hold references to delegates without preventing garbage collection of the owner — essential for event-based architectures.

```csharp
using CreativeCoders.Core.Weak;

// Create a weak action — owner can still be GC'd
var action = new WeakAction(myHandler.OnEvent);

if (action.IsAlive())
{
    action.Execute();
}

// Weak function with return value
var func = new WeakFunc<string>(() => "result");
string result = func.Execute();

// Control owner lifetime explicitly
var keepAlive = new WeakAction(handler.OnEvent, KeepOwnerAliveMode.KeepAlive);
var autoGuess = new WeakAction(handler.OnEvent, KeepOwnerAliveMode.AutoGuess);
```

### Error — Error Handler Abstraction

```csharp
using CreativeCoders.Core.Error;

// Delegate-based error handler
IErrorHandler handler = new DelegateErrorHandler(ex =>
    Console.WriteLine($"Error: {ex.Message}"));

handler.HandleException(new InvalidOperationException("Something went wrong"));

// Null error handler (no-op, useful as default)
IErrorHandler nullHandler = new NullErrorHandler();
```

### Dependencies — Dependency Resolution

Resolve and sort dependencies within a graph, with circular reference detection.

```csharp
using CreativeCoders.Core.Dependencies;

// Build dependency graph
var collection = new DependencyObjectCollection<string>();
collection.Add(new DependencyObject<string>("app", new[] { "database", "cache" }));
collection.Add(new DependencyObject<string>("database", new[] { "config" }));
collection.Add(new DependencyObject<string>("cache", new[] { "config" }));
collection.Add(new DependencyObject<string>("config", Array.Empty<string>()));

// Resolve all dependencies for an element
var resolver = new DependencyResolver<string>(collection);
var deps = resolver.Resolve("app"); // ["database", "cache", "config"]

// Sort by dependency order
var sorter = new DependencySorter<string>(collection);
var sorted = sorter.Sort(); // config, database, cache, app
```

## API Reference

| Namespace | Purpose |
|-----------|---------|
| `CreativeCoders.Core` | Core validation (`Ensure`), extensions, disposables, fluent helpers |
| `CreativeCoders.Core.Caching` | In-memory cache with expiration and region support |
| `CreativeCoders.Core.Chaining` | Chain of Responsibility pattern |
| `CreativeCoders.Core.Collections` | LINQ extensions, observable collections, list/dictionary helpers |
| `CreativeCoders.Core.Comparing` | Function-based comparers and multi-key equality |
| `CreativeCoders.Core.Dependencies` | Dependency graph resolution and sorting |
| `CreativeCoders.Core.Enums` | Enum-to-string conversion and flag enumeration |
| `CreativeCoders.Core.EnsureArguments` | Fluent argument validation with `Argument<T>` |
| `CreativeCoders.Core.Error` | Error handler abstraction |
| `CreativeCoders.Core.IO` | File system extensions, path safety, directory helpers |
| `CreativeCoders.Core.Messaging` | Pub/Sub messenger with weak references |
| `CreativeCoders.Core.ObjectLinking` | Bi-directional property binding between objects |
| `CreativeCoders.Core.Placeholders` | Template string placeholder replacement |
| `CreativeCoders.Core.Reflection` | Type discovery, generic instance creation, expression utilities |
| `CreativeCoders.Core.SysEnvironment` | Testable `System.Environment` abstraction |
| `CreativeCoders.Core.Text` | String extensions, pattern matching, JSON serialization |
| `CreativeCoders.Core.Threading` | Thread-safe collections, synchronized values, locking mechanisms |
| `CreativeCoders.Core.Visitors` | Visitor pattern infrastructure |
| `CreativeCoders.Core.Weak` | Weak reference delegates |
