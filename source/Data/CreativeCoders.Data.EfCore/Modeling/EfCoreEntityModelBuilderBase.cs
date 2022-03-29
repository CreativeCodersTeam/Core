using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreativeCoders.Data.EfCore.Modeling;

[PublicAPI]
public abstract class EfCoreEntityModelBuilderBase<TEntity> : IEfCoreEntityModelBuilder
    where TEntity : class
{
    public void BuildModel(ModelBuilder modelBuilder)
    {
        BuildEntity(modelBuilder.Entity<TEntity>());
    }

    protected abstract void BuildEntity(EntityTypeBuilder<TEntity> entity);
}