using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core.Dependencies;

/// <summary>
/// Resolves all transitive dependencies for an element within a <see cref="DependencyObjectCollection{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the elements in the dependency graph.</typeparam>
public class DependencyResolver<T>
    where T : class
{
    private readonly DependencyObjectCollection<T> _dependencyObjectCollection;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyResolver{T}"/> class.
    /// </summary>
    /// <param name="dependencyObjectCollection">The collection of dependency objects to resolve against.</param>
    public DependencyResolver(DependencyObjectCollection<T> dependencyObjectCollection)
    {
        _dependencyObjectCollection = dependencyObjectCollection;
    }

    /// <summary>
    /// Resolves all unique transitive dependencies for the specified element.
    /// </summary>
    /// <param name="element">The element whose dependencies are resolved.</param>
    /// <returns>A distinct collection of all dependencies for <paramref name="element"/>.</returns>
    public IEnumerable<T> Resolve(T element)
    {
        return InternalResolve(element).Distinct().ToArray();
    }

    private IEnumerable<T> InternalResolve(T element)
    {
        var dependencyObject = _dependencyObjectCollection.GetDependencyObject(element);

        return GetDependencies(dependencyObject, false);
    }

    private static IEnumerable<T> GetDependencies(DependencyObject<T> dependencyObject, bool addRootData)
    {
        if (addRootData)
        {
            yield return dependencyObject.Element;
        }

        foreach (var dependency in dependencyObject.DependsOn)
        {
            yield return dependency.Element;

            var dependencies = GetDependencies(dependency, true);

            foreach (var subDependency in dependencies)
            {
                yield return subDependency;
            }
        }
    }
}
