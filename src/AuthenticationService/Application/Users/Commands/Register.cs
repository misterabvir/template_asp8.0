using Application.Common.Repositories;
using Application.Common.Services;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using FluentValidation;
using MediatR;
using Shared.Results;

namespace Application.Users.Commands;

public static class Register
{
    public sealed record Command(string Username, string Email, string Password) : IRequest<Result>;
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Username).NotEmpty().Matches(Username.Regex);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().Matches(Email.Regex);
            RuleFor(x => x.Password).NotEmpty().Matches(Password.Regex);
        }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IEncryptService encryptService) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await unitOfWork.Users.IsUsernameNotUnique(request.Username, cancellationToken))
            {
                return Errors.Users.UsernameAlreadyTaken;
            }
            if (await unitOfWork.Users.IsEmailNotUnique(request.Email, cancellationToken))
            {
                return Errors.Users.EmailAlreadyTaken;
            }

            var salt = encryptService.GenerateSalt();
            var password = encryptService.Encrypt(request.Password, salt);
            var user = User.Create(
                Username.Create(request.Username),
                Email.Create(request.Email),
                Password.Create(password),
                Salt.Create(salt));

            await unitOfWork.Users.AddAsync(user, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}