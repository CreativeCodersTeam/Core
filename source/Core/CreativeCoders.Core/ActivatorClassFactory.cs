using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core
{
    [PublicAPI]
    public class ActivatorClassFactory : IClassFactory
    {
        public object Create(Type classType)
        {
            return Activator.CreateInstance(classType);
        }

        public object Create(Type classType, Action<object> setupInstance)
        {
            var instance = Activator.CreateInstance(classType);

            setupInstance(instance);

            return instance;
        }

        public TClass Create<TClass>()
            where TClass : class
        {
            return Activator.CreateInstance<TClass>();
        }

        public TClass Create<TClass>(Action<TClass> setupInstance)
            where TClass : class
        {
            var instance = Activator.CreateInstance<TClass>();

            setupInstance(instance);

            return instance;
        }
    }

    [PublicAPI]
    public class ActivatorClassFactory<T> : ActivatorClassFactory, IClassFactory<T>
        where T : class
    {
        public T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public T Create(Action<T> setupInstance)
        {
            var instance = Activator.CreateInstance<T>();

            setupInstance(instance);

            return instance;
        }
    }
}