using AuthenticationService.Application.Common.Repositories;
using AuthenticationService.Domain.UserAggregate;
using AuthenticationService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Infrastructure.Repositories;

internal class UserRepository(AuthenticationDbContext context) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Data.Email == email, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<bool> IsEmailNotUnique(string email, CancellationToken cancellationToken = default)
    {
        return await context.Users.AnyAsync(u => u.Data.Email == email, cancellationToken);
    }

    public async Task<bool> IsUsernameNotUnique(string username, CancellationToken cancellationToken = default)
    {
        return await context.Users.AnyAsync(u => u.Data.Username == username, cancellationToken);
    }
}
