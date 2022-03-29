using System.Collections.Generic;

namespace CreativeCoders.Core.Dependencies;

///-------------------------------------------------------------------------------------------------
/// <summary>   A dependency object holding the element and its dependencies. </summary>
///
/// <typeparam name="T">    Generic type parameter of the <see cref="DependencyObject{T}.Element"/>. </typeparam>
///-------------------------------------------------------------------------------------------------
public class DependencyObject<T>
    where T : class
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Initializes a new instance of the <see cref="DependencyObject{T}"/> class. </summary>
    ///
    /// <param name="element">  The element. </param>
    ///-------------------------------------------------------------------------------------------------
    public DependencyObject(T element)
    {
        Element = element;
            
        DependsOn = new List<DependencyObject<T>>();
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the element. </summary>
    ///
    /// <value> The element. </value>
    ///-------------------------------------------------------------------------------------------------
    public T Element { get; }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the objects it is dependent on. </summary>
    ///
    /// <value> A list of <see cref="DependencyObject{T}"/> this object depends on. </value>
    ///-------------------------------------------------------------------------------------------------
    public List<DependencyObject<T>> DependsOn { get; }
}