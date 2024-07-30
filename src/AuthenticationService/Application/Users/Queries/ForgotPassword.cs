using Application.Common.Repositories;
using Application.Common.Services;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using FluentValidation;
using MediatR;
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
        IUnitOfWork unitOfWork, 
        IVerificationService verificationService) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken: cancellationToken);
            if (user is null)
            {
                return Errors.Users.NotFound;
            }

            var result = await verificationService.SendVerificationCodeAsync(user.Id, user.Data.Username, user.Data.Email, cancellationToken);
            return result.IsFailure ? (Result)result.Error : Result.Success();
        }
    }
}