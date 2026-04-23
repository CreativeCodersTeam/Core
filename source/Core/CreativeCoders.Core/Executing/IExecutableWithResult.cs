using JetBrains.Annotations;

namespace CreativeCoders.Core.Executing;

/// <summary>
/// Defines an executable operation with no parameters that returns a strongly-typed result.
/// </summary>
/// <typeparam name="T">The type of the result.</typeparam>
[PublicAPI]
public interface IExecutableWithResult<out T>
{
    /// <summary>
    /// Executes the operation and returns the result.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    T Execute();
}
