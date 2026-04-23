using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Error;

/// <summary>
/// Implements <see cref="IErrorHandler"/> by delegating exception handling to a provided <see cref="Action{T}"/>.
/// </summary>
[PublicAPI]
public class DelegateErrorHandler : IErrorHandler
{
    private readonly Action<Exception> _handleException;

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateErrorHandler"/> class.
    /// </summary>
    /// <param name="handleException">The delegate invoked to handle exceptions.</param>
    public DelegateErrorHandler(Action<Exception> handleException)
    {
        Ensure.IsNotNull(handleException);

        _handleException = handleException;
    }

    /// <inheritdoc />
    public void HandleException(Exception exception)
    {
        _handleException(exception);
    }
}
