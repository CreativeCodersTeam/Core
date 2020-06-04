using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core.Dependencies {

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A dependency tree node. </summary>
    ///
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    ///-------------------------------------------------------------------------------------------------
    public class DependencyTreeNode<T>
        where T : class
    {
        internal DependencyTreeNode(T element, IEnumerable<DependencyTreeNode<T>> subNodes)
        {
            Element = element;
            SubNodes = subNodes.ToArray();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the element of the tree node. </summary>
        ///
        /// <value> The element. </value>
        ///-------------------------------------------------------------------------------------------------
        public T Element { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the sub nodes. </summary>
        ///
        /// <value> The sub nodes. </value>
        ///-------------------------------------------------------------------------------------------------
        public IReadOnlyCollection<DependencyTreeNode<T>> SubNodes { get; }
    }
}