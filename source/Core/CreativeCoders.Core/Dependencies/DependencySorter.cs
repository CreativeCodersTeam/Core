using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core.Collections;

namespace CreativeCoders.Core.Dependencies;

/// <summary>
/// Performs a topological sort on elements in a <see cref="DependencyObjectCollection{T}"/> based on their dependencies.
/// </summary>
/// <typeparam name="T">The type of the elements to sort.</typeparam>
public class DependencySorter<T>
    where T : class
{
    private readonly DependencyObjectCollection<T> _dependencyObjectCollection;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencySorter{T}"/> class.
    /// </summary>
    /// <param name="dependencyObjectCollection">The collection of dependency objects to sort.</param>
    public DependencySorter(DependencyObjectCollection<T> dependencyObjectCollection)
    {
        _dependencyObjectCollection = dependencyObjectCollection;
    }

    /// <summary>
    /// Sorts the elements in topological order so that dependencies appear before the elements that depend on them.
    /// </summary>
    /// <returns>The elements sorted in dependency order, with the least dependent elements first.</returns>
    /// <exception cref="CircularReferenceException">The dependency graph contains a circular reference.</exception>
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
                    sortObjects.Select(object (x) => x.Element).ToArray());
            }

            var elements = objectsWithoutDependencies.Select(x => x.Element).ToArray();

            sortedList.AddRange(elements);

            sortObjects.Remove(objectsWithoutDependencies);

            sortObjects.ForEach(x => x.DependsOn.Remove(elements));
        }

        return sortedList;
    }
}
