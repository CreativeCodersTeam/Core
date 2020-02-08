using System;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Di
{
    [PublicAPI]
    public class DiContainerClassFactory : IClassFactory
    {
        private readonly IDiContainer _diContainer;

        public DiContainerClassFactory(IDiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public object Create(Type classType)
        {
            return _diContainer.GetInstance(classType);
        }

        public object Create(Type classType, Action<object> setupInstance)
        {
            var instance = _diContainer.GetInstance(classType);

            setupInstance(instance);

            return instance;
        }

        public T Create<T>() where T : class
        {
            return _diContainer.GetInstance<T>();
        }

        public T Create<T>(Action<T> setupInstance) where T : class
        {
            var instance = _diContainer.GetInstance<T>();

            setupInstance(instance);

            return instance;
        }
    }

    [PublicAPI]
    public class DiContainerClassFactory<T> : DiContainerClassFactory, IClassFactory<T>
        where T : class
    {
        private readonly IDiContainer _diContainer;

        public DiContainerClassFactory(IDiContainer diContainer) : base(diContainer)
        {
            _diContainer = diContainer;
        }

        public T Create()
        {
            return _diContainer.GetInstance<T>();
        }

        public T Create(Action<T> setupInstance)
        {
            var instance = _diContainer.GetInstance<T>();

            setupInstance(instance);

            return instance;
        }
    }
}