using System;

namespace CreativeCoders.Di.Exceptions;

public class ServiceLocatorNotInitializedException : Exception
{
    public ServiceLocatorNotInitializedException() : base("ServiceLocator is not initialized!") { }
}
