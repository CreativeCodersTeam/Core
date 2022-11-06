namespace CreativeCoders.DependencyInjection;

/// <summary>
/// Factory for retrieving or creating object instances
/// </summary>
/// <typeparam name="T">Object type to retrieve or create</typeparam>
public interface IObjectFactory<out T>
{
    /// <summary>
    /// Gets the instance of an object from service provider or creates it resolving ctor parameters
    /// via service provider
    /// </summary>
    /// <returns></returns>
    T GetInstance();

    /// <summary>
    /// Creates an instance of an object resolving ctor parameters via service provider and
    /// <paramref name="parameters"/>
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    T CreateInstance(params object[] parameters);
}
