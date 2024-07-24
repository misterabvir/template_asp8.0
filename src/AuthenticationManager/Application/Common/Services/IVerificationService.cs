
using Domain.UserAggregate;

using Shared.Results;

namespace Application.Common.Services;

public interface IVerificationService
{
    public int VerificationCodeLength { get; }

    Task<Result> SendVerificationCodeAsync(User user, CancellationToken cancellationToken);
    Task<Result> VerifyAsync(User user, string code, CancellationToken cancellationToken);
}