using JetBrains.Annotations;

namespace CreativeCoders.Core.Executing;

/// <summary>
/// Defines an executable operation that accepts a strongly-typed parameter and returns a strongly-typed result.
/// </summary>
/// <typeparam name="TParameter">The type of the parameter.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
[PublicAPI]
public interface IExecutableWithResult<in TParameter, out TResult>
{
    /// <summary>
    /// Executes the operation with the specified parameter and returns the result.
    /// </summary>
    /// <param name="parameter">The parameter for the operation.</param>
    /// <returns>The result of the operation.</returns>
    TResult Execute(TParameter parameter);
}
