using System.Data.Common;
using MySql.Data.MySqlClient;
namespace Infrastructure.Persistence;

public class DbConnectionFactory(string connectionString)
{
    public DbConnection CreateConnection()
    {
        return new MySqlConnection(connectionString);
    }
}
