namespace AuthenticationService.Application.Common.Repositories;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollBackTransactionAsync(CancellationToken cancellationToken = default);
}
