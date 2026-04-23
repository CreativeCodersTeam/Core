using JetBrains.Annotations;

namespace CreativeCoders.Core.Executing;

/// <summary>
/// Defines an executable operation that accepts a strongly-typed parameter and has no return value.
/// </summary>
/// <typeparam name="T">The type of the parameter.</typeparam>
[PublicAPI]
public interface IExecutable<in T>
{
    /// <summary>
    /// Executes the operation with the specified parameter.
    /// </summary>
    /// <param name="parameter">The parameter for the operation.</param>
    void Execute(T parameter);
}
