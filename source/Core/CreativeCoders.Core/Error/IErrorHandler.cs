using System;

namespace CreativeCoders.Core.Error;

/// <summary>   Interface for an error handler. </summary>
public interface IErrorHandler
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Handles the exception <paramref name="exception"/>. </summary>
    ///
    /// <param name="exception">    The exception that gets handled. </param>
    ///-------------------------------------------------------------------------------------------------
    void HandleException(Exception exception);
}
