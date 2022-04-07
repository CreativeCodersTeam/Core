using System;

namespace CreativeCoders.Net.WebApi.Exceptions;

public abstract class ApiException : Exception
{
    protected ApiException(string message) : base(message) { }
}
