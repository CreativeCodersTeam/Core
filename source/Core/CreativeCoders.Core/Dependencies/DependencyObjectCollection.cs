using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreativeCoders.Core.Collections;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Dependencies;

///-------------------------------------------------------------------------------------------------
/// <summary>   Collection of dependency objects. </summary>
///
/// <typeparam name="T">    Generic type parameter of the elements. </typeparam>
///-------------------------------------------------------------------------------------------------
[PublicAPI]
public class DependencyObjectCollection<T>
    where T : class
{
    private List<DependencyObject<T>> _dependencyObjects;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="DependencyObjectCollection{T}"/> class.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public DependencyObjectCollection()
    {
        _dependencyObjects = new List<DependencyObject<T>>();
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Adds an element to the collection and returns the corresponding
    ///     <see cref="DependencyObject{T}"/>. If element is already added, returns the already
    ///     existing <see cref="DependencyObject{T}"/>.
    /// </summary>
    ///
    /// <param name="element">  The element to add. </param>
    ///
    /// <returns>   A <see cref="DependencyObject{T}"/> </returns>
    ///-------------------------------------------------------------------------------------------------
    public DependencyObject<T> AddElement(T element)
    {
        if (element == null)
        {
            return null;
        }

        var existingDependencyObject = _dependencyObjects.FirstOrDefault(x => x.Element.Equals(element));

        if (existingDependencyObject != null)
        {
            return existingDependencyObject;
        }
            
        var newDependencyObject = new DependencyObject<T>(element);
            
        _dependencyObjects.Add(newDependencyObject);

        return newDependencyObject;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Adds a dependency to the collection. </summary>
    ///
    /// <param name="element">              The element to add. </param>
    /// <param name="dependsOnElements">    A list of elements the <paramref name="element"/> depends
    ///                                     on. </param>
    ///-------------------------------------------------------------------------------------------------
    public void AddDependency(T element, params T[] dependsOnElements)
    {
        var dependencyObject = AddElement(element);
        var dependsOnObjects = dependsOnElements.Select(AddElement);

        dependsOnObjects.ForEach(x =>
        {
            if (!dependencyObject.DependsOn.Contains(x))
            {
                dependencyObject.DependsOn.Add(x);
            }
        });
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets dependency object corresponding to the <paramref name="element"/>. </summary>
    ///
    /// <param name="element">  The element for which the <see cref="DependencyObject{T}"/> is
    ///                         returned. </param>
    ///
    /// <returns>
    ///     The dependency object. If <paramref name="element"/> is not in the collection, null is
    ///     returned.
    /// </returns>
    ///-------------------------------------------------------------------------------------------------
    public DependencyObject<T> GetDependencyObject(T element)
    {
        return _dependencyObjects.FirstOrDefault(x => x.Element.Equals(element));
    }

    /// <summary>   Removes redundant dependencies. </summary>
    public void RemoveRedundancies()
    {
        foreach (var dependencyObject in _dependencyObjects)
        {
            RemoveRedundancies(dependencyObject.DependsOn);
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Determines if there are circular references in the collection. </summary>
    ///
    /// <returns>
    ///     True if there are circular references, false if the collection has no circular references.
    /// </returns>
    ///-------------------------------------------------------------------------------------------------
    public bool CheckForCircularReferences()
    {
        var sortObjects = _dependencyObjects
            .Select(x => new SortObject<T>(x)).ToList();

        while (sortObjects.Count > 0)
        {
            var objectsWithoutDependencies = sortObjects.Where(x => !x.DependsOn.Any()).ToArray();

            if (objectsWithoutDependencies.Length == 0)
            {
                return true;
            }

            var elements = objectsWithoutDependencies.Select(x => x.Element).ToArray();

            sortObjects.Remove(objectsWithoutDependencies);

            sortObjects.ForEach(x => x.DependsOn.Remove(elements));
        }

        return false;
    }

    [SuppressMessage("ReSharper", "LoopCanBePartlyConvertedToQuery")]
    private void RemoveRedundancies(IList<DependencyObject<T>> dependencyObjects)
    {
        foreach (var dependency in dependencyObjects.ToArray())
        {
            var otherDependencies = dependencyObjects.Where(x => x != dependency).ToArray();

            if (otherDependencies.Any(x => ObjectIsSubObject(x.DependsOn, dependency)))
            {
                dependencyObjects.Remove(dependency);
            }
        }

        foreach (var dependingObject in dependencyObjects)
        {
            RemoveRedundancies(dependingObject.DependsOn);
        }
    }

    private bool ObjectIsSubObject(IReadOnlyCollection<DependencyObject<T>> dependencyObjects, DependencyObject<T> dependencyObject)
    {
        if (dependencyObjects.Contains(dependencyObject))
        {
            return true;
        }

        return dependencyObjects
            .Any(childDependencyObject =>
                ObjectIsSubObject(childDependencyObject.DependsOn, dependencyObject));
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the dependency objects. </summary>
    ///
    /// <value> The dependency objects. </value>
    ///-------------------------------------------------------------------------------------------------
    public IReadOnlyCollection<DependencyObject<T>> DependencyObjects => _dependencyObjects;
}