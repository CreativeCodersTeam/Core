using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace CreativeCoders.Data.Nhibernate;

[PublicAPI]
public static class ServiceCollectionExtensions
{
    public static void AddNhibernateSupport(this IServiceCollection services,
        Action<Configuration> setupNhibernateConfiguration)
    {
        var configuration = new Configuration();
        configuration.DataBaseIntegration(dbi =>
        {
            dbi.ConnectionString = "";
            dbi.Dialect<MsSql2008Dialect>();
            dbi.Driver<Sql2008ClientDriver>();
        });
        setupNhibernateConfiguration(configuration);

        services.AddScoped(_ => configuration.BuildSessionFactory());
    }
}
