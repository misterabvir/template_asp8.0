


using MailService.Domain;

namespace MailService.Application.Common.Repositories;

public interface IUserRepository
{
    Task<User> GetOrCreateAsync(Guid userId, string email, string username, CancellationToken cancellationToken = default);
}

public interface IMessageRepository
{
    Task AddMessageAsync(Guid userId, string type, string reason, CancellationToken cancellationToken);
    Task<IEnumerable<Message>> GetLastEmails(int amount, CancellationToken cancellationToken);
    Task<IEnumerable<Message>> GetLastUserEmails(Guid userId, int amount, CancellationToken cancellationToken);
}