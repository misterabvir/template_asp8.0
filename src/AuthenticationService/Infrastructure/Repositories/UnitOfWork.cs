using Application.Common.Repositories;

using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Repositories;

internal class UnitOfWork(AuthenticationDbContext context, IUserRepository userRepository) : IUnitOfWork
{
    public IUserRepository Users => userRepository;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await context.Database.CommitTransactionAsync(cancellationToken);   
    }

    public async Task RollBackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await context.Database.RollbackTransactionAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
