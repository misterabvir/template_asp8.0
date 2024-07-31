using System.Data;
using System.Data.Common;

using Dapper;

using MySql.Data.MySqlClient;
namespace MailService.Infrastructure.Persistence;

public class DbConnectionFactory
{
    private readonly string _connectionString;
    
    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
        SqlMapper.AddTypeHandler(new GuidTypeHandler());
    }
    
    
    public DbConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }
}

internal class GuidTypeHandler : SqlMapper.TypeHandler<object>
{
    public override object? Parse(object value)
    {
        return Guid.Parse(value.ToString()!);
    }

    public override void SetValue(IDbDataParameter parameter, object? value)
    {
        parameter.Value = value?.ToString();
    }
}