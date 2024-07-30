


using Application.Common.Repositories;

using Dapper;

using Domain;

using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class UserRepository(DbConnectionFactory dbConnectionFactory) : IUserRepository
{
    public async Task<User> GetOrCreateAsync(Guid userId, string email, string username, CancellationToken cancellationToken = default)
    {
        using var connection = dbConnectionFactory.CreateConnection();
        await connection.OpenAsync();
        string sql = "SELECT user_id AS UserId FROM `users` WHERE user_id = @UserId";
        var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserId = userId, Username = username });
        if (user is not null)
        {
            return user;
        }

        sql = "INSERT INTO `users` (user_id, email, username) VALUES (@UserId,@Email, @Username)";
        await connection.ExecuteAsync(sql, new { Email = email, Username = username });

        return await connection.QueryFirstAsync<User>(sql, new {UserId = userId, Email = email, Username = username });
    }
}
