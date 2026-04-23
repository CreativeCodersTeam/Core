using JetBrains.Annotations;

namespace CreativeCoders.Core.Executing;

/// <summary>
/// Defines an executable operation with no parameters and no return value.
/// </summary>
[PublicAPI]
public interface IExecutable
{
    /// <summary>
    /// Executes the operation.
    /// </summary>
    void Execute();
}
