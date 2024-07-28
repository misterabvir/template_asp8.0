
using Domain.UserAggregate;

namespace Application.Common.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid initiatorUserId, CancellationToken cancellationToken = default);
    Task<bool> IsEmailNotUnique(string email, CancellationToken cancellationToken = default);
    Task<bool> IsUsernameNotUnique(string username, CancellationToken cancellationToken = default);
}
