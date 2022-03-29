using System;

namespace CreativeCoders.Core.Dependencies;

///-------------------------------------------------------------------------------------------------
/// <summary>
///     Exception for signalling a circular reference in the dependency collection.
/// </summary>
///
/// <seealso cref="Exception"/>
///-------------------------------------------------------------------------------------------------
public class CircularReferenceException : Exception
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="CircularReferenceException"/> class.
    /// </summary>
    ///
    /// <param name="message">                      The message. </param>
    /// <param name="possibleCircularReferences">   The possible circular references. </param>
    ///-------------------------------------------------------------------------------------------------
    public CircularReferenceException(string message, object[] possibleCircularReferences) : base(message)
    {
        PossibleCircularReferences = possibleCircularReferences;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the possible circular references. </summary>
    ///
    /// <value>
    ///     The possible circular references. Contains the elements left over, which can be
    ///     responsible for the circular reference.
    /// </value>
    ///-------------------------------------------------------------------------------------------------
    public object[] PossibleCircularReferences { get; }
}