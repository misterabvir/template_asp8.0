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

internal class GuidTypeHandler :SqlMapper.TypeHandler<Guid>
{
    public override void SetValue(IDbDataParameter parameter, Guid guid)
    {
        parameter.Value = guid.ToString();
    }

    public override Guid Parse(object value)
    {
        return new Guid(value.ToString()!);
    }
}