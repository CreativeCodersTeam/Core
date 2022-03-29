using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core.Dependencies;

///-------------------------------------------------------------------------------------------------
/// <summary>   A dependency resolver, which resolves dependencies for a element. </summary>
///
/// <typeparam name="T">    Generic type parameter of the element. </typeparam>
///-------------------------------------------------------------------------------------------------
public class DependencyResolver<T>
    where T : class
{
    private readonly DependencyObjectCollection<T> _dependencyObjectCollection;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="DependencyResolver{T}"/> class.
    /// </summary>
    ///
    /// <param name="dependencyObjectCollection">   Collection of dependency objects. </param>
    ///-------------------------------------------------------------------------------------------------
    public DependencyResolver(DependencyObjectCollection<T> dependencyObjectCollection)
    {
        _dependencyObjectCollection = dependencyObjectCollection;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Resolves all dependencies for <paramref name="element"/>. </summary>
    ///
    /// <param name="element">  The element. </param>
    ///
    /// <returns>
    ///     The dependencies for <paramref name="element"/>. Only unique dependencies, duplicates are
    ///     removed.
    /// </returns>
    ///-------------------------------------------------------------------------------------------------
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