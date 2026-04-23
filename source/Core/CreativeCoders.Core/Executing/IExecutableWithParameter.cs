using JetBrains.Annotations;

namespace CreativeCoders.Core.Executing;

/// <summary>
/// Defines an executable operation that accepts an untyped parameter and has no return value.
/// </summary>
[PublicAPI]
public interface IExecutableWithParameter
{
    /// <summary>
    /// Executes the operation with the specified parameter.
    /// </summary>
    /// <param name="parameter">The parameter for the operation.</param>
    void Execute(object parameter);
}
