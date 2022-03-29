using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Data;

[PublicAPI]
public interface IPersistRepository<in TEntity>
    where TEntity : class
{
    void Add(TEntity entity);

    void Add(IEnumerable<TEntity> entities);

    void Update(TEntity entity);

    void Update(IEnumerable<TEntity> entities);

    void Delete(TEntity entity);

    void Delete(IEnumerable<TEntity> entities);
}
