


using Application.Common.Repositories;

using Dapper;

using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class MessageRepository(DbConnectionFactory dbConnectionFactory) : IMessageRepository
{
    public async Task AddMessageAsync(Guid userId, string type, string reason, CancellationToken cancellationToken = default)
    {
        using var connection = dbConnectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        string sql = "INSERT INTO `messages` (recipient_id, type, reason) VALUES (@UserId, @Type, @Reason)";
        await connection.ExecuteAsync(sql, new { UserId = userId, Type = type, Reason = reason });
    }
}