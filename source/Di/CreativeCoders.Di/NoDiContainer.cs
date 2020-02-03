using System;
using System.Collections.Generic;
using CreativeCoders.Di.Exceptions;

namespace CreativeCoders.Di
{
    public class NoDiContainer : IDiContainer
    {
        public T GetInstance<T>()
            where T : class
        {
            throw new ServiceLocatorNotInitializedException();
        }

        public object GetInstance(Type serviceType)
        {
            throw new ServiceLocatorNotInitializedException();
        }

        public T GetInstance<T>(string name)
            where T : class
        {
            throw new ServiceLocatorNotInitializedException();
        }

        public object GetInstance(Type serviceType, string name)
        {
            throw new ServiceLocatorNotInitializedException();
        }

        public IEnumerable<T> GetInstances<T>()
            where T : class
        {
            throw new ServiceLocatorNotInitializedException();
        }

        public IEnumerable<object> GetInstances(Type serviceType)
        {
            throw new ServiceLocatorNotInitializedException();
        }

        public bool TryGetInstance<T>(out T instance) where T : class
        {
            throw new ServiceLocatorNotInitializedException();
        }

        public bool TryGetInstance(Type serviceType, out object instance)
        {
            throw new ServiceLocatorNotInitializedException();
        }

        public IDiContainerScope CreateScope()
        {
            throw new ServiceLocatorNotInitializedException();
        }

        public object GetService(Type serviceType)
        {
            throw new ServiceLocatorNotInitializedException();
        }
    }
}