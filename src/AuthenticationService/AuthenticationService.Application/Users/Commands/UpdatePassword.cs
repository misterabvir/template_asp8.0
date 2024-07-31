using AuthenticationService.Application.Common.Repositories;
using AuthenticationService.Application.Common.Services;
using AuthenticationService.Domain.UserAggregate;
using AuthenticationService.Domain.UserAggregate.ValueObjects;
using FluentValidation;
using MediatR;
using Shared.Results;

namespace AuthenticationService.Application.Users.Commands;

public static class UpdatePassword
{
    public sealed record Command(Guid UserId, string Password) : IRequest<Result>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().Matches(Password.Regex);
        }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IEncryptService encryptService) : IRequestHandler<Command, Result>
    {

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
            if(user is null){
                return Errors.Users.NotFound;
            }

            var salt = encryptService.GenerateSalt();
            var password = encryptService.Encrypt(request.Password, salt);

            user.UpdatePassword(Password.Create(password), Salt.Create(salt));

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}