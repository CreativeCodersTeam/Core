using System;

namespace CreativeCoders.Core.Error;

/// <summary>
/// Defines a contract for handling exceptions.
/// </summary>
public interface IErrorHandler
{
    /// <summary>
    /// Handles the specified exception.
    /// </summary>
    /// <param name="exception">The exception to handle.</param>
    void HandleException(Exception exception);
}
