using AuthenticationService.Application.Common.Repositories;
using AuthenticationService.Application.Common.Services;
using AuthenticationService.Domain.UserAggregate;
using AuthenticationService.Domain.UserAggregate.ValueObjects;
using FluentValidation;
using MediatR;
using Shared.Results;

namespace AuthenticationService.Application.Users.Commands;

public static class ResetPassword
{
    public sealed record Command(string Email, string Password, string Code) :
    IRequest<Result<(User User, string Token)>>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(IVerificationService verificationService)
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().Matches(Email.Regex);
            RuleFor(x => x.Password).NotEmpty().Matches(Password.Regex);
            RuleFor(x => x.Code).NotEmpty().Length(verificationService.VerificationCodeLength);
        }
    }

    public sealed class Handler(
        IUnitOfWork unitOfWork,
        IVerificationService verificationService,
        IEncryptService encryptService,
        ITokenService tokenService) :
        IRequestHandler<Command, Result<(User User, string Token)>>
    {
        public async Task<Result<(User User, string Token)>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);
            if (user is null)
            {
                return Errors.Users.NotFound;
            }
            var verifyResult = await verificationService.VerifyAsync(user.Id, request.Code, cancellationToken);
            if (verifyResult.IsFailure)
            {
                return verifyResult.Error;
            }

            var salt = encryptService.GenerateSalt();
            var password = encryptService.Encrypt(request.Password, salt);
            user.UpdatePassword(Password.Create(password), Salt.Create(salt));
            await unitOfWork.SaveChangesAsync(cancellationToken);
            var token = tokenService.GenerateToken(user);
            return (user, token);
        }
    }

}