using System.Data;
using System.Data.Common;

using Dapper;

using MySql.Data.MySqlClient;
namespace MailService.Infrastructure.Persistence;

public class DbConnectionFactory : IDisposable
{
    private readonly string _connectionString;
    private DbConnection? _connection;

    public string ConnectionString => _connectionString;

    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
        SqlMapper.AddTypeHandler(new GuidTypeHandler());
    }


    public DbConnection CreateConnection() => _connection ??= new MySqlConnection(_connectionString);

    public void Dispose() => _connection?.Close();
}

internal class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
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