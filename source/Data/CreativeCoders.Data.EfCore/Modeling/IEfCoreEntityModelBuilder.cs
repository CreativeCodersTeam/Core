using Microsoft.EntityFrameworkCore;

namespace CreativeCoders.Data.EfCore.Modeling;

public interface IEfCoreEntityModelBuilder
{
    void BuildModel(ModelBuilder modelBuilder);
}
