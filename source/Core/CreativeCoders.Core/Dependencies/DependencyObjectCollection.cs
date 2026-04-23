using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreativeCoders.Core.Collections;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Dependencies;

/// <summary>
/// Represents a collection of <see cref="DependencyObject{T}"/> instances that models a dependency graph.
/// </summary>
/// <typeparam name="T">The type of the elements in the dependency graph.</typeparam>
[PublicAPI]
public class DependencyObjectCollection<T>
    where T : class
{
    private readonly List<DependencyObject<T>> _dependencyObjects;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyObjectCollection{T}"/> class.
    /// </summary>
    public DependencyObjectCollection()
    {
        _dependencyObjects = [];
    }

    /// <summary>
    /// Adds an element to the collection and returns the corresponding <see cref="DependencyObject{T}"/>.
    /// If the element already exists, returns the existing <see cref="DependencyObject{T}"/>.
    /// </summary>
    /// <param name="element">The element to add.</param>
    /// <returns>
    /// The <see cref="DependencyObject{T}"/> for the element, or <see langword="null"/> if
    /// <paramref name="element"/> is <see langword="null"/>.
    /// </returns>
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

    /// <summary>
    /// Adds a dependency relationship where <paramref name="element"/> depends on the specified elements.
    /// </summary>
    /// <param name="element">The element that has dependencies.</param>
    /// <param name="dependsOnElements">The elements that <paramref name="element"/> depends on.</param>
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

    /// <summary>
    /// Gets the <see cref="DependencyObject{T}"/> corresponding to the specified element.
    /// </summary>
    /// <param name="element">The element to look up.</param>
    /// <returns>
    /// The <see cref="DependencyObject{T}"/> for the element, or <see langword="null"/> if the element
    /// is not in the collection.
    /// </returns>
    public DependencyObject<T> GetDependencyObject(T element)
    {
        return _dependencyObjects.FirstOrDefault(x => x.Element.Equals(element));
    }

    /// <summary>
    /// Removes redundant (transitively implied) dependencies from all objects in the collection.
    /// </summary>
    public void RemoveRedundancies()
    {
        foreach (var dependencyObject in _dependencyObjects)
        {
            RemoveRedundancies(dependencyObject.DependsOn);
        }
    }

    /// <summary>
    /// Checks whether the collection contains any circular references.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if circular references exist; otherwise, <see langword="false"/>.
    /// </returns>
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
    private static void RemoveRedundancies(List<DependencyObject<T>> dependencyObjects)
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

    private static bool ObjectIsSubObject(IReadOnlyCollection<DependencyObject<T>> dependencyObjects,
        DependencyObject<T> dependencyObject)
    {
        if (dependencyObjects.Contains(dependencyObject))
        {
            return true;
        }

        return dependencyObjects
            .Any(childDependencyObject =>
                ObjectIsSubObject(childDependencyObject.DependsOn, dependencyObject));
    }

    /// <summary>
    /// Gets the dependency objects in the collection.
    /// </summary>
    public IReadOnlyCollection<DependencyObject<T>> DependencyObjects => _dependencyObjects;
}
