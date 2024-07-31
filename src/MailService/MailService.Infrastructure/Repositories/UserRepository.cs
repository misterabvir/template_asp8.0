using MailService.Application.Common.Repositories;

using Dapper;

using MailService.Domain;

using MailService.Infrastructure.Persistence;


namespace MailService.Infrastructure.Repositories;

public class UserRepository(DbConnectionFactory dbConnectionFactory) : IUserRepository
{
    private const int DefaultAbsoluteExpiration = 30;
    private const int SemaphoreLockWaitTimeOut = 1000;
    private readonly static SemaphoreSlim Semaphore = new(1, 1);

    public async Task<User> GetOrCreateAsync(Guid userId, string email, string username, CancellationToken cancellationToken = default)
    {
        using var connection = dbConnectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        string sqlSelect = "SELECT user_id AS UserId, email As Email, username AS Username FROM `users` WHERE user_id = @UserId";
        var user = await connection.QueryFirstOrDefaultAsync<User>(sqlSelect, new { UserId = userId });
        if (user is not null)
        {
            return user;
        }

        var hasLock = await Semaphore.WaitAsync(SemaphoreLockWaitTimeOut, cancellationToken);
        if (!hasLock)
        {
            throw new Exception("Failed to acquire semaphore lock");
        }
        try
        {
            user = await connection.QueryFirstOrDefaultAsync<User>(sqlSelect, new { UserId = userId });
            if (user is not null)
            {
                return user;
            }
            var sqlInsert = "INSERT INTO `users` (user_id, email, username) VALUES (@UserId,@Email, @Username)";
            await connection.ExecuteAsync(sqlInsert, new { UserId = userId, Email = email, Username = username });

            user = new User { UserId = userId, Email = email, Username = username };
        }
        finally
        {
            Semaphore.Release();
        }
        return user;
    }
}
