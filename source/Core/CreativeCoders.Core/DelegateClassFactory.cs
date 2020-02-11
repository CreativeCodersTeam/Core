using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core
{
    [PublicAPI]
    public class DelegateClassFactory : IClassFactory
    {
        private readonly Func<Type, object> _createInstance;

        public DelegateClassFactory(Func<Type, object> createInstance)
        {
            Ensure.IsNotNull(createInstance, nameof(createInstance));
            
            _createInstance = createInstance;
        }
        
        public object Create(Type classType)
        {
            return _createInstance(classType);
        }

        public object Create(Type classType, Action<object> setupInstance)
        {
            var instance = Create(classType);

            setupInstance(instance);

            return instance;
        }

        public T Create<T>() where T : class
        {
            return (T) Create(typeof(T));
        }

        public T Create<T>(Action<T> setupInstance) where T : class
        {
            var instance = Create<T>();

            setupInstance(instance);

            return instance;
        }
    }
    
    [PublicAPI]
    public class DelegateClassFactory<T> : DelegateClassFactory, IClassFactory<T>
        where T : class
    {
        private readonly Func<T> _createInstance;

        public DelegateClassFactory(Func<T> createInstance) : base(type => CreateClassObject(type, createInstance))
        {
            _createInstance = createInstance;
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static object CreateClassObject(Type classType, Func<T> createInstance)
        {
            if (typeof(T) != classType)
            {
                throw new InvalidOperationException(
                    $"Class type '{classType.FullName}' not possible for generic delegate class factory for type '{typeof(T).FullName}'");
            }

            return createInstance();
        }

        public T Create()
        {
            return _createInstance();
        }

        public T Create(Action<T> setupInstance)
        {
            var instance = _createInstance();

            setupInstance(instance);

            return instance;
        }
    }
}