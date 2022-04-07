using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core;

///-------------------------------------------------------------------------------------------------
/// <summary>
///     An implementation of <see cref="IClassFactory"/> using <see cref="Activator"/> for
///     creating the objects.
/// </summary>
///
/// <seealso cref="IClassFactory"/>
///-------------------------------------------------------------------------------------------------
[PublicAPI]
public class ActivatorClassFactory : IClassFactory
{
    /// <inheritdoc />
    public object Create(Type classType)
    {
        return Activator.CreateInstance(classType);
    }

    /// <inheritdoc />
    public object Create(Type classType, Action<object> setupInstance)
    {
        var instance = Activator.CreateInstance(classType);

        setupInstance(instance);

        return instance;
    }

    /// <inheritdoc />
    public TClass Create<TClass>()
        where TClass : class
    {
        return Activator.CreateInstance<TClass>();
    }

    /// <inheritdoc />
    public TClass Create<TClass>(Action<TClass> setupInstance)
        where TClass : class
    {
        var instance = Activator.CreateInstance<TClass>();

        setupInstance(instance);

        return instance;
    }
}

///-------------------------------------------------------------------------------------------------
/// <summary>   An implementation of <see cref="IClassFactory{T}"/> using <see cref="Activator"/> for
///     creating the objects. </summary>
///
/// <typeparam name="T">    Generic type parameter. </typeparam>
///
/// <seealso cref="ActivatorClassFactory"/>
/// <seealso cref="IClassFactory{T}"/>
///-------------------------------------------------------------------------------------------------
[PublicAPI]
public class ActivatorClassFactory<T> : ActivatorClassFactory, IClassFactory<T>
    where T : class
{
    /// <inheritdoc />
    public T Create()
    {
        return Activator.CreateInstance<T>();
    }

    /// <inheritdoc />
    public T Create(Action<T> setupInstance)
    {
        var instance = Activator.CreateInstance<T>();

        setupInstance(instance);

        return instance;
    }
}
