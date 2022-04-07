using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core;

///-------------------------------------------------------------------------------------------------
/// <summary>
///     An implementation of <see cref="IClassFactory"/> using functions to create objects.
/// </summary>
///
/// <seealso cref="IClassFactory"/>
///-------------------------------------------------------------------------------------------------
[PublicAPI]
public class DelegateClassFactory : IClassFactory
{
    private readonly Func<Type, object> _createInstance;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the CreativeCoders.Core.DelegateClassFactory class. The
    ///     <see cref="Func{T, TResult}"/> is used to create the objects.
    /// </summary>
    ///
    /// <param name="createInstance">   The <see cref="Func{T, TResult}"/> for creating new
    ///                                 instances. </param>
    ///-------------------------------------------------------------------------------------------------
    public DelegateClassFactory(Func<Type, object> createInstance)
    {
        Ensure.IsNotNull(createInstance, nameof(createInstance));

        _createInstance = createInstance;
    }

    /// <inheritdoc />
    public object Create(Type classType)
    {
        return _createInstance(classType);
    }

    /// <inheritdoc />
    public object Create(Type classType, Action<object> setupInstance)
    {
        var instance = Create(classType);

        setupInstance(instance);

        return instance;
    }

    /// <inheritdoc />
    public T Create<T>() where T : class
    {
        return (T) Create(typeof(T));
    }

    /// <inheritdoc />
    public T Create<T>(Action<T> setupInstance) where T : class
    {
        var instance = Create<T>();

        setupInstance(instance);

        return instance;
    }
}

///-------------------------------------------------------------------------------------------------
/// <summary>   An implementation of <see cref="IClassFactory"/> using functions to create objects. </summary>
///
/// <typeparam name="T">    Type of the class to create. </typeparam>
///
/// <seealso cref="DelegateClassFactory"/>
/// <seealso cref="IClassFactory{T}"/>
///-------------------------------------------------------------------------------------------------
[PublicAPI]
public class DelegateClassFactory<T> : DelegateClassFactory, IClassFactory<T>
    where T : class
{
    private readonly Func<T> _createInstance;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the CreativeCoders.Core.DelegateClassFactory class. The
    ///     <see cref="Func{TResult}"/> is used to create the objects.
    /// </summary>
    ///
    /// <param name="createInstance">   The <see cref="Func{TResult}"/> for creating new
    ///                                 instances. </param>
    ///-------------------------------------------------------------------------------------------------
    public DelegateClassFactory(Func<T> createInstance) : base(
        type => CreateClassObject(type, createInstance))
    {
        _createInstance = createInstance;
    }

    private static object CreateClassObject(Type classType, Func<T> createInstance)
    {
        if (typeof(T) != classType)
        {
            throw new InvalidOperationException(
                $"Class type '{classType.FullName}' not possible for generic delegate class factory for type '{typeof(T).FullName}'");
        }

        return createInstance();
    }

    /// <inheritdoc />
    public T Create()
    {
        return _createInstance();
    }

    /// <inheritdoc />
    public T Create(Action<T> setupInstance)
    {
        var instance = _createInstance();

        setupInstance(instance);

        return instance;
    }
}
