using System;
using CreativeCoders.Di.Building;
using JetBrains.Annotations;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace CreativeCoders.Data.Nhibernate
{
    [PublicAPI]
    public static class ContainerBuilderExtensions
    {
        public static IDiContainerBuilder AddNhibernateSupport(this IDiContainerBuilder builder, Action<Configuration> setupNhibernateConfiguration)
        {
            var configuration = new Configuration();
            configuration.DataBaseIntegration(dbi =>
            {
                dbi.ConnectionString = "";
                dbi.Dialect<MsSql2008Dialect>();
                dbi.Driver<Sql2008ClientDriver>();
            });
            setupNhibernateConfiguration(configuration);
            
            builder.AddScoped(_ => configuration.BuildSessionFactory());

            return builder;
        }
    }
}
