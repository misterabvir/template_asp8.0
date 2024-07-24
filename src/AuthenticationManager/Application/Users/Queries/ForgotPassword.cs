using Application.Common.Services;
using Domain.Persistence.Contexts;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace Application.Users.Queries;

public static class ForgotPassword
{
    public sealed record Query(string Email) : IRequest<Result>;

    public sealed class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().Matches(Email.Regex);
        }
    }

    public sealed class Handler(
        AuthenticationDbContext context, 
        IVerificationService verificationService) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(u=>u.Data.Email == request.Email, cancellationToken: cancellationToken);
            if (user is null)
            {
                return Errors.Users.NotFound;
            }

            var result = await verificationService.SendVerificationCodeAsync(user, cancellationToken);
            return result.IsFailure ? (Result)result.Error : Result.Success();
        }

    }
}