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
Ensure.Argument(instance).NotNull(); // throws ArgumentNullException if instance is null
Ensure.Argument(nameof(str), str).IsNullOrEmpty(); // throws ArgumentException if str is null or empty
Ensure.Argument(nameof(items), items).IsNullOrEmpty(); // throws ArgumentException if items is null or empty
Ensure.Argument(nameof(fileName), fileName).FileExists(); // throws ArgumentException if fileName not exists
Ensure.Argument(nameof(dirName), dirName).DirectoryExists(); // throws ArgumentException if dirName not exists
```

## Threading
Thread-safe collections, synchronization primitives

## Enums
Enum extensions and helpers

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

## Null objects
Null objects for various types
