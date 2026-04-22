using System.Collections.Generic;

namespace CreativeCoders.Core.Dependencies;

/// <summary>
/// Represents an element and its dependencies within a dependency graph.
/// </summary>
/// <typeparam name="T">The type of the element.</typeparam>
public class DependencyObject<T>
    where T : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyObject{T}"/> class.
    /// </summary>
    /// <param name="element">The element to wrap.</param>
    public DependencyObject(T element)
    {
        Element = element;

        DependsOn = [];
    }

    /// <summary>
    /// Gets the wrapped element.
    /// </summary>
    public T Element { get; }

    /// <summary>
    /// Gets the list of <see cref="DependencyObject{T}"/> instances this object depends on.
    /// </summary>
    public List<DependencyObject<T>> DependsOn { get; }
}
