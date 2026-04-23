using System;

namespace CreativeCoders.Core.Visitors;

/// <summary>
/// The exception that is thrown when a visitable object does not have a matching accept method
/// for the given visitor type and the visitor's <see cref="IVisitorInfo.ThrowIfNoAcceptMethod"/>
/// property is <see langword="true"/>.
/// </summary>
public class AcceptForVisitorNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AcceptForVisitorNotFoundException"/> class.
    /// </summary>
    /// <param name="visitorName">The fully qualified name of the visitor type that was not accepted.</param>
    public AcceptForVisitorNotFoundException(string visitorName) : base(
        $"Visitable object has no accept method for visitor '{visitorName}'") { }
}
