using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core.Dependencies;

/// <summary>
/// Represents a node in a dependency tree, containing an element and its child dependency nodes.
/// </summary>
/// <typeparam name="T">The type of the element in the tree node.</typeparam>
public class DependencyTreeNode<T>
    where T : class
{
    internal DependencyTreeNode(T element, IEnumerable<DependencyTreeNode<T>> subNodes)
    {
        Element = element;
        SubNodes = subNodes.ToArray();
    }

    /// <summary>
    /// Gets the element associated with this tree node.
    /// </summary>
    public T Element { get; }

    /// <summary>
    /// Gets the child nodes representing the dependencies of this node's element.
    /// </summary>
    public IReadOnlyCollection<DependencyTreeNode<T>> SubNodes { get; }
}
