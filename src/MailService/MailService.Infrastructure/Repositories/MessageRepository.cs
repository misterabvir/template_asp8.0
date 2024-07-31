using MailService.Application.Common.Repositories;

using Dapper;

using MailService.Domain;

using MailService.Infrastructure.Persistence;

namespace MailService.Infrastructure.Repositories;

public class MessageRepository(DbConnectionFactory dbConnectionFactory) : IMessageRepository
{
    public async Task AddMessageAsync(Guid userId, string type, string reason, CancellationToken cancellationToken = default)
    {
        using var connection = dbConnectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        var message = new Message() {RecipientId= userId, Type = type, Reason = reason };
        string sql = @"INSERT INTO `messages` (message_id, recipient_id, type, reason, created_at) 
                        VALUES (@MessageId, @RecipientId, @Type, @Reason, @CreatedAt)";
        await connection.ExecuteAsync(sql, message);
    }

    public async Task<IEnumerable<Message>> GetLastEmails(int amount, CancellationToken cancellationToken)
    {
        using var connection = dbConnectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
         string sql = @"SELECT  
                m.message_id AS MessageId,
                m.recipient_id AS RecipientId,
                m.type AS Type,
                m.reason AS Reason,
                m.created_at AS CreatedAt,
                u.user_id AS UserId,
                u.username AS Username,
                u.email AS Email
            FROM `messages` AS m
            INNER JOIN users u ON u.user_id = m.recipient_id
            ORDER BY created_at 
            DESC LIMIT @Amount";
        var messages = await connection.QueryAsync<Message, User, Message>(sql, (message, user)=>{
             message.User = user;
             return message;   
        },
        splitOn: "user_id",
        param: new {Amount = amount});
        return messages;  
    }

    public async Task<IEnumerable<Message>> GetLastUserEmails(Guid userId, int amount, CancellationToken cancellationToken)
    {
        using var connection = dbConnectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        string sql = @"SELECT  
                m.message_id AS MessageId,
                m.recipient_id AS UserId,
                m.type AS Type,
                m.reason AS Reason,
                m.created_at AS CreatedAt,
                u.user_id AS UserId,
                u.username AS Username,
                u.email AS Email
            FROM `messages` AS m
            WHERE recipient_id = @UserId 
            INNER JOIN users u ON u.user_id = m.recipient_id
            ORDER BY created_at 
            DESC LIMIT @Amount";
        var messages = await connection.QueryAsync<Message, User, Message>(sql, (message, user)=>{
             message.User = user;
             return message;   
        },
        splitOn: "user_id",
        param: new {Amount = amount, UserId = userId});
        return messages;    
    }
}