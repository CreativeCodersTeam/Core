using CreativeCoders.Config.Base;

namespace CreativeCoders.Data.EfCore.SqlServer;

public class SqlServerConnectionStringSetting : ISetting<SqlServerConnectionString>
{
    public SqlServerConnectionStringSetting(string connectionString)
    {
        Value = new SqlServerConnectionString{ConnectionString = connectionString};
    }

    public SqlServerConnectionString Value { get; }
}