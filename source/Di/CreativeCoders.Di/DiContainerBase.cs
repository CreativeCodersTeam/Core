using System;
using CreativeCoders.Di.Exceptions;

namespace CreativeCoders.Di
{
    public abstract class DiContainerBase
    {
        protected static T Resolve<T, TException>(Func<T> resolveFunc)
            where TException : Exception
        {
            try
            {
                return resolveFunc();
            }
            catch (TException ex)
            {
                throw new ResolveFailedException(typeof(T), ex);
            }
        }

        protected static object Resolve<TException>(Type serviceType, Func<object> resolveFunc)
            where TException : Exception
        {
            try
            {
                return resolveFunc();
            }
            catch (TException ex)
            {
                throw new ResolveFailedException(serviceType, ex);
            }
        }        
    }
}