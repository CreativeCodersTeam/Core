# Core .NET libraries

Following parts are covered by this repository:

* [Core](source/Core/README.md): Core library
  * Arguments ensuring: Ensure arguments are as expected
  * Threading: Thread-safe collections, synchronization primitives
  * Enums: Enum extensions and helpers
  * Visitor pattern: Visitor pattern implementation
  * IO: Static helpers for IO operations based on System.IO.Abstractions as a replacement for File, Directory, Path, etc.
  * Weak action and functions: Weak delegates for actions and functions
  * Reflection: Classes and extensions for working with dynamic code
  * ObjectLinking: Classes for linking objects together, so that properties of one object are automatically updated when properties of another object change
  * Dependency tree builder: Classes for building and resolving dependency trees
  * SysEnvironment: Abstraction for Environment class to enable mocking for unit tests
  * Null objects: Null objects for various types
