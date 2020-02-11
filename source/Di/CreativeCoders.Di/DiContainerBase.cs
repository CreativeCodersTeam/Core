using System;
using CreativeCoders.Di.Exceptions;

namespace CreativeCoders.Di
{
    public abstract class DiContainerBase
    {
        protected static T Resolve<T, TException>(Func<T> resolve)
            where TException : Exception
        {
            try
            {
                return resolve();
            }
            catch (TException ex)
            {
                throw new ResolveFailedException(typeof(T), ex);
            }
        }

        protected static object Resolve<TException>(Type serviceType, Func<object> resolve)
            where TException : Exception
        {
            try
            {
                return resolve();
            }
            catch (TException ex)
            {
                throw new ResolveFailedException(serviceType, ex);
            }
        }        
    }
}