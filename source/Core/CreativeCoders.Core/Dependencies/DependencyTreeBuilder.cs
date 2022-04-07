using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core.Dependencies;

///-------------------------------------------------------------------------------------------------
/// <summary>   A dependency tree builder. </summary>
///
/// <typeparam name="T">    Generic type parameter of the elements. </typeparam>
///-------------------------------------------------------------------------------------------------
public class DependencyTreeBuilder<T>
    where T : class
{
    private readonly DependencyObjectCollection<T> _dependencyObjectCollection;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="DependencyTreeBuilder{T}"/> class.
    /// </summary>
    ///
    /// <param name="dependencyObjectCollection">   Collection of dependency objects. </param>
    ///-------------------------------------------------------------------------------------------------
    public DependencyTreeBuilder(DependencyObjectCollection<T> dependencyObjectCollection)
    {
        _dependencyObjectCollection = dependencyObjectCollection;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Builds the dependency tree for the given <paramref name="element"/>. </summary>
    ///
    /// <param name="element">  The element for which the dependency tree should be build. </param>
    ///
    /// <returns>
    ///     The root node of the dependency tree. The root node is the given
    ///     <paramref name="element"/> and all dependencies are arranged below.
    /// </returns>
    ///-------------------------------------------------------------------------------------------------
    public DependencyTreeNode<T> Build(T element)
    {
        return new(element, GetSubNodes(_dependencyObjectCollection.GetDependencyObject(element)));
    }

    private static IEnumerable<DependencyTreeNode<T>> GetSubNodes(DependencyObject<T> dependencyObject)
    {
        return dependencyObject
            .DependsOn
            .Select(dependency =>
                new DependencyTreeNode<T>(dependency.Element, GetSubNodes(dependency)));
    }
}
