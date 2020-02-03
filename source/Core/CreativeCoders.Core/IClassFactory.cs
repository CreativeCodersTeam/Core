using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core
{
    [PublicAPI]
    public interface IClassFactory
    {
        object Create(Type classType);

        object Create(Type classType, Action<object> setupInstance);

        T Create<T>()
            where T : class;

        T Create<T>(Action<T> setupInstance)
            where T : class;
    }

    [PublicAPI]
    public interface IClassFactory<out T> : IClassFactory
        where T : class
    {
        T Create();

        T Create(Action<T> setupInstance);
    }
}