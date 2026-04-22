using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core.Dependencies;

/// <summary>
/// Builds a hierarchical dependency tree from a <see cref="DependencyObjectCollection{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the elements in the dependency tree.</typeparam>
public class DependencyTreeBuilder<T>
    where T : class
{
    private readonly DependencyObjectCollection<T> _dependencyObjectCollection;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyTreeBuilder{T}"/> class.
    /// </summary>
    /// <param name="dependencyObjectCollection">The collection of dependency objects to build the tree from.</param>
    public DependencyTreeBuilder(DependencyObjectCollection<T> dependencyObjectCollection)
    {
        _dependencyObjectCollection = dependencyObjectCollection;
    }

    /// <summary>
    /// Builds a dependency tree rooted at the specified element.
    /// </summary>
    /// <param name="element">The root element of the dependency tree.</param>
    /// <returns>
    /// A <see cref="DependencyTreeNode{T}"/> representing the root, with all dependencies arranged as sub-nodes.
    /// </returns>
    public DependencyTreeNode<T> Build(T element)
    {
        return new DependencyTreeNode<T>(element, GetSubNodes(_dependencyObjectCollection.GetDependencyObject(element)));
    }

    private static IEnumerable<DependencyTreeNode<T>> GetSubNodes(DependencyObject<T> dependencyObject)
    {
        return dependencyObject
            .DependsOn
            .Select(dependency =>
                new DependencyTreeNode<T>(dependency.Element, GetSubNodes(dependency)));
    }
}
