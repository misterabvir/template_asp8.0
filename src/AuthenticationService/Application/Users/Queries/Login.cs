using Application.Common.Repositories;
using Application.Common.Services;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using FluentValidation;
using MediatR;
using Shared.Results;

namespace Application.Users.Queries;

public static class Login
{
    public sealed record Query(string Email, string Password) : IRequest<Result<(User User, string Token)>>;
    public sealed class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().Matches(Email.Regex);
            RuleFor(x => x.Password).NotEmpty().Matches(Password.Regex);
        }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, ITokenService tokenService, IEncryptService encryptService) :
        IRequestHandler<Query, Result<(User User, string Token)>>
    {
        public async Task<Result<(User User, string Token)>> Handle(Query query, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetByEmailAsync(query.Email, cancellationToken: cancellationToken);
            if (user is null)
            {
                return Errors.Users.InvalidCredentials;
            }

            if (user.Status != Status.Active)
            {
                return Errors.Users.InvalidCredentials;
            }

            var password = encryptService.Encrypt(query.Password, user.Data.Salt.Value);
            if (user.Data.Password.IsSameAs(password) == false)
            {
                return Errors.Users.InvalidCredentials;
            }

            var token = tokenService.GenerateToken(user);

            return (user, token);
        }
    }
}