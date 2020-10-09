using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core
{
    /// <summary>   An interface for a generic class factory. </summary>
    [PublicAPI]
    public interface IClassFactory
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a new object of type <paramref name="classType"/>. </summary>
        ///
        /// <param name="classType">    Type of the class to create. </param>
        ///
        /// <returns>   An instance of the type <paramref name="classType"/>. </returns>
        ///-------------------------------------------------------------------------------------------------
        object Create(Type classType);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a new object of type <paramref name="classType"/>. </summary>
        ///
        /// <param name="classType">        Type of the class to create. </param>
        /// <param name="setupInstance">    <see cref="Action{T}"/> that gets called to setup created
        ///                                 instance. </param>
        ///
        /// <returns>   An instance of the type <paramref name="classType"/>. </returns>
        ///-------------------------------------------------------------------------------------------------
        object Create(Type classType, Action<object> setupInstance);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a new object of type <typeparamref name="T"/>. </summary>
        ///
        /// <typeparam name="T">    Type of the class to create. </typeparam>
        ///
        /// <returns>   An instance of the type <typeparamref name="T"/>. </returns>
        ///-------------------------------------------------------------------------------------------------
        T Create<T>()
            where T : class;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a new object of type <typeparamref name="T"/>. </summary>
        ///
        /// <typeparam name="T">    Type of the class to create. </typeparam>
        /// <param name="setupInstance">    <see cref="Action{T}"/> that gets called to setup created
        ///                                                                 instance. </param>
        ///
        /// <returns>   An instance of the type <typeparamref name="T"/>. </returns>
        ///-------------------------------------------------------------------------------------------------
        T Create<T>(Action<T> setupInstance)
            where T : class;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for a generic class factory for objects of type <typeparamref name="T" />.
    /// </summary>
    ///
    /// <typeparam name="T">    Type of the class to create. </typeparam>
    ///-------------------------------------------------------------------------------------------------
    [PublicAPI]
    public interface IClassFactory<out T> : IClassFactory
        where T : class
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a new object of type <typeparamref name="T"/>. </summary>
        ///
        /// <returns>   An instance of the type <typeparamref name="T"/>. </returns>
        ///-------------------------------------------------------------------------------------------------
        T Create();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a new object of type <typeparamref name="T"/>. </summary>
        ///
        /// <param name="setupInstance">    <see cref="Action{T}"/> that gets called to setup created
        ///                                 instance. </param>
        ///
        /// <returns>   An instance of the type <typeparamref name="T"/>. </returns>
        ///-------------------------------------------------------------------------------------------------
        T Create(Action<T> setupInstance);
    }
}