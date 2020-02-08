using CreativeCoders.Data.EfCore.Modeling;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace CreativeCoders.Data.EfCore
{
    [PublicAPI]
    public class EfCoreDbContext : DbContext
    {
        private readonly IEfCoreEntityModelBuilderSource _entityModelBuilderSource;

        public EfCoreDbContext(IEfCoreEntityModelBuilderSource entityModelBuilderSource)
        {
            _entityModelBuilderSource = entityModelBuilderSource;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnEntityModelBuilderSourceSetup(_entityModelBuilderSource);

            var modelBuilders = _entityModelBuilderSource.GetEntityModelBuilders();

            foreach (var entityModelBuilder in modelBuilders)
            {
                entityModelBuilder.BuildModel(modelBuilder);
            }
        }

        protected virtual void OnEntityModelBuilderSourceSetup(IEfCoreEntityModelBuilderSource entityModelBuilderSource)
        {

        }
    }
}