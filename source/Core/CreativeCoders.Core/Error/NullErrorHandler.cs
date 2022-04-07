using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Error;

///-------------------------------------------------------------------------------------------------
/// <summary>   An error handler which simply does nothing. </summary>
///
/// <seealso cref="IErrorHandler"/>
///-------------------------------------------------------------------------------------------------
[PublicAPI]
[ExcludeFromCodeCoverage]
public class NullErrorHandler : IErrorHandler
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Static instance for use, when a <see cref="NullErrorHandler"/> is needed. </summary>
    ///
    /// <value> The instance. </value>
    ///-------------------------------------------------------------------------------------------------
    public static IErrorHandler Instance { get; } = new NullErrorHandler();

    /// <inheritdoc />
    public void HandleException(Exception exception) { }
}
