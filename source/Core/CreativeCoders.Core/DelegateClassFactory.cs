using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core
{
    [PublicAPI]
    public class DelegateClassFactory : IClassFactory
    {
        private readonly Func<Type, object> _creatorFunc;

        public DelegateClassFactory(Func<Type, object> creatorFunc)
        {
            Ensure.IsNotNull(creatorFunc, nameof(creatorFunc));
            
            _creatorFunc = creatorFunc;
        }
        
        public object Create(Type classType)
        {
            return _creatorFunc(classType);
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
        private readonly Func<T> _creatorFunc;

        public DelegateClassFactory(Func<T> creatorFunc) : base(type => CreateClassObject(type, creatorFunc))
        {
            _creatorFunc = creatorFunc;
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static object CreateClassObject(Type classType, Func<T> creatorFunc)
        {
            if (typeof(T) != classType)
            {
                throw new InvalidOperationException(
                    $"Class type '{classType.FullName}' not possible for generic delegate class factory for type '{typeof(T).FullName}'");
            }

            return creatorFunc();
        }

        public T Create()
        {
            return _creatorFunc();
        }

        public T Create(Action<T> setupInstance)
        {
            var instance = _creatorFunc();

            setupInstance(instance);

            return instance;
        }
    }
}