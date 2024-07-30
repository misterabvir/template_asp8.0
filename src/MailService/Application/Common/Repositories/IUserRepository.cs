

using Domain;

namespace Application.Common.Repositories;

public interface IUserRepository
{
    Task<User> GetOrCreateAsync(Guid userId, string email, string username, CancellationToken cancellationToken = default);
}

public interface IMessageRepository
{
    Task AddMessageAsync(Guid userId, string type, string reason, CancellationToken cancellationToken);
}