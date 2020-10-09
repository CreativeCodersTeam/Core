using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core.Dependencies
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A dependency sorter. </summary>
    ///
    /// <typeparam name="T">    Generic type parameter of teh elements. </typeparam>
    ///-------------------------------------------------------------------------------------------------
    public class DependencySorter<T>
        where T : class
    {
        private readonly DependencyObjectCollection<T> _dependencyObjectCollection;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes a new instance of the <see cref="DependencySorter{T}"/> class. </summary>
        ///
        /// <param name="dependencyObjectCollection">   Collection of dependency objects. </param>
        ///-------------------------------------------------------------------------------------------------
        public DependencySorter(DependencyObjectCollection<T> dependencyObjectCollection)
        {
            _dependencyObjectCollection = dependencyObjectCollection;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sorts the elements. </summary>
        ///
        /// <exception cref="CircularReferenceException">   Thrown when a Circular Reference error
        ///                                                 condition occurs. </exception>
        ///
        /// <returns>
        ///     Sorted list of the elements respecting the dependencies. First element is the least
        ///     depending element.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public IEnumerable<T> Sort()
        {
            var sortedList = new List<T>();
            
            var sortObjects = _dependencyObjectCollection
                .DependencyObjects
                .Select(x => new SortObject<T>(x)).ToList();

            while (sortObjects.Count > 0)
            {
                var objectsWithoutDependencies = sortObjects.Where(x => !x.DependsOn.Any()).ToArray();

                if (objectsWithoutDependencies.Length == 0)
                {
                    throw new CircularReferenceException("Circular reference detected",
                        sortObjects.Select(x => (object) x.Element).ToArray());
                }

                var elements = objectsWithoutDependencies.Select(x => x.Element).ToArray();
                
                sortedList.AddRange(elements);
                
                sortObjects.Remove(objectsWithoutDependencies);
                
                sortObjects.ForEach(x => x.DependsOn.Remove(elements));
            }

            return sortedList;
        }
    }
}