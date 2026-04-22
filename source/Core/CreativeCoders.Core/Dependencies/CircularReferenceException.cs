using System;

namespace CreativeCoders.Core.Dependencies;

/// <summary>
/// Represents an exception that is thrown when a circular reference is detected in a dependency collection.
/// </summary>
/// <seealso cref="Exception"/>
public class CircularReferenceException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CircularReferenceException"/> class.
    /// </summary>
    /// <param name="message">The error message that describes the circular reference.</param>
    /// <param name="possibleCircularReferences">The elements that may be involved in the circular reference.</param>
    public CircularReferenceException(string message, object[] possibleCircularReferences) : base(message)
    {
        PossibleCircularReferences = possibleCircularReferences;
    }

    /// <summary>
    /// Gets the elements that may be responsible for the circular reference.
    /// </summary>
    public object[] PossibleCircularReferences { get; }
}
