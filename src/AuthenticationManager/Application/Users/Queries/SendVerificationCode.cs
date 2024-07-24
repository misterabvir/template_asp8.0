using Application.Common.Services;

using Domain.Persistence.Contexts;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Shared.Results;

namespace Application.Users.Queries;

public static class SendVerificationCode
{
    public sealed record Query(string Email) : IRequest<Result>;
    public sealed class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(q => q.Email).NotEmpty().EmailAddress().Matches(Email.Regex);
        }
    }

    public sealed class Handler(AuthenticationDbContext context, IVerificationService verificationService) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
        {
            var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Data.Email == query.Email, cancellationToken: cancellationToken);
            if (user is null)
            {
                return Errors.Users.NotFound;
            }

            if (user.Status == Status.Active)
            {
                return Errors.Users.AlreadyActive;
            }

            var result = await verificationService.SendVerificationCodeAsync(user, cancellationToken);
            return result!;
        }
    }
}