# Core .NET library

Basic classes and interfaces for .NET applications. 

## Arguments ensuring
Ensure arguments are as expected

### Simple argument checks for null, empty, etc.
[Ensure.cs](CreativeCoders.Core/Ensure.cs)
```csharp
// Argument name can be given via second parameter or via nameof. If none is given, the name of variable is used.
Ensure.NotNull(instance); // throws ArgumentNullException if instance is null
// Ensure.IsNullOrEmpty works for strings and IEnumerable<T>
Ensure.IsNullOrEmpty(str); // throws ArgumentException if str is null or empty
Ensure.IsNullOrEmpty(items); // throws ArgumentException if items is null or empty
Ensure.FileExists(fileName); // throws ArgumentException if fileName not exists
Ensure.DirectoryExists(dirName); // throws ArgumentException if dirName not exists
// for more checks see Ensure.cs
```
### More complex argument checks for null, empty, etc.
[Ensure.Argument Extensions](CreativeCoders.Core/EnsureArguments/Extensions)
```csharp
// Argument name can be given via second parameter or via nameof. If none is given, the name of variable is used.
// you can chain the checks to ensure multiple conditions at once
Ensure.Argument(instance).NotNull(); // throws ArgumentNullException if instance is null
Ensure.Argument(text).IsNullOrEmpty(); // throws ArgumentException if str is null or empty
Ensure.Argument(items).IsNullOrEmpty(); // throws ArgumentException if items is null or empty
Ensure.Argument(fileName).FileExists(); // throws ArgumentException if fileName not exists
Ensure.Argument(dirName).DirectoryExists(); // throws ArgumentException if dirName not exists
Ensure.Argument(text).NotNull().HasMaxLength(maxLength); // throws if text is null or exceeds max length
```

## Threading
Thread-safe collections, synchronization primitives

## Enums
Enum extensions and helpers

```csharp
enum MyEnum
{
    [EnumStringValue("Value1")]
    ValueOne,
    [EnumStringValue("Value2")]
    ValueTwo
}

// Instantiate a new EnumStringConverter
var enumConverter = new EnumStringConverter();

// Convert enum to string
string enumString = enumConverter.Convert(MyEnum.ValueOne); 
Console.WriteLine(enumString); // Outputs: Value1

// Convert string to enum
MyEnum enumValue = enumConverter.Convert<MyEnum>("Value2");
Console.WriteLine(enumValue); // Outputs: ValueTwo
```

## Visitor pattern
Visitor pattern implementation

## IO
Static helpers for IO operations based on System.IO.Abstractions as a replacement for File, Directory, Path, etc.

## Weak action and functions
Weak delegates for actions and functions

## Reflection
Classes and extensions for working with dynamic code

## ObjectLinking
Classes for linking objects together, so that properties of one object are automatically updated when properties of another object change

## Dependency tree builder
Classes for building and resolving dependency trees

## SysEnvironment
Abstraction for Environment class to enable mocking for unit tests

#### Use static methods from Env
```csharp
string desktopPath = Env.GetFolderPath(Environment.SpecialFolder.Desktop);
```

#### Use IEnvironment via DI
```csharp
// First register service
services.AddEnvironment();

// Sample app
public class MyApp
{
    private readonly IEnvironment _environment;

    public MyApp(IEnvironment environment)
    {
        _environment = environment;
    }

    public void Run()
    {
        // Use the IEnvironment instance
        Console.WriteLine("Current Directory: " + _environment.CurrentDirectory);
        Console.WriteLine("Machine Name: " + _environment.MachineName);
        Console.WriteLine("User Name: " + _environment.UserName);

        Console.WriteLine("Environment Variables:");
        foreach (var envVar in _environment.GetEnvironmentVariables())
        {
            Console.WriteLine($"Key: {envVar.Key}, Value: {envVar.Value}");
        }
    }
}
```

## Null objects
Null objects for various types
