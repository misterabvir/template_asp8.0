
using Domain.UserAggregate;

using Shared.Results;

namespace Application.Common.Services;

public interface IVerificationService
{
    public int VerificationCodeLength { get; }

    Task<Result> SendVerificationCodeAsync(Guid userId, string username, string email, CancellationToken cancellationToken);
    Task<Result> VerifyAsync(Guid userId, string code, CancellationToken cancellationToken);
}