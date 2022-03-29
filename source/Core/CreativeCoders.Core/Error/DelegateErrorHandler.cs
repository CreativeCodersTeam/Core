using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Error;

[PublicAPI]
public class DelegateErrorHandler : IErrorHandler
{
    private readonly Action<Exception> _handleException;

    public DelegateErrorHandler(Action<Exception> handleException)
    {
        Ensure.IsNotNull(handleException, nameof(handleException));
            
        _handleException = handleException;
    }
        
    public void HandleException(Exception exception)
    {
        _handleException(exception);
    }
}