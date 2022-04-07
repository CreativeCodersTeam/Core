using System;

namespace CreativeCoders.UnitTests.Net.Http;

///-------------------------------------------------------------------------------------------------
/// <summary>
///     Exception thrown if no <see cref="MockHttpResponder"/>, which is responsible for the
///     request, was found.
/// </summary>
///
/// <seealso cref="Exception"/>
///-------------------------------------------------------------------------------------------------
public class NoResponderFoundException : Exception
{
    internal NoResponderFoundException() : base("No matching responder found") { }
}
