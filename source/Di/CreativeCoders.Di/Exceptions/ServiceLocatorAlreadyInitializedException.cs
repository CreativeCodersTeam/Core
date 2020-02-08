using System;

namespace CreativeCoders.Di.Exceptions
{
    public class ServiceLocatorAlreadyInitializedException : Exception
    {
        public ServiceLocatorAlreadyInitializedException() : base("Service locator is already initialized") {}
    }
}
