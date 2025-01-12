using AuthenticationService.Application.Common.Repositories;
using AuthenticationService.Application.Common.Services;
using AuthenticationService.Domain.UserAggregate;
using AuthenticationService.Domain.UserAggregate.ValueObjects;
using FluentValidation;
using MediatR;
using Domain.Results;

namespace AuthenticationService.Application.Users.Queries;

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

    public sealed class Handler(IUnitOfWork unitOfWork, IVerificationService verificationService) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetByEmailAsync(query.Email, cancellationToken: cancellationToken);
            if (user is null)
            {
                return Errors.Users.NotFound;
            }

            var result = await verificationService.SendVerificationCodeAsync(user.Id, user.Data.Username, user.Data.Email, cancellationToken);
            return result!;
        }
    }
}