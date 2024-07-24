using Application.Common.Services;
using Domain.Persistence.Contexts;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Shared.Results;

namespace Application.Users.Commands;

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
        AuthenticationDbContext context,
        IVerificationService verificationService,
        IEncryptService encryptService,
        ITokenService tokenService) :
        IRequestHandler<Command, Result<(User User, string Token)>>
    {
        public async Task<Result<(User User, string Token)>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Data.Email == request.Email, cancellationToken);
            if (user is null)
            {
                return Errors.Users.NotFound;
            }
            var verifyResult = await verificationService.VerifyAsync(user, request.Code, cancellationToken);
            if (verifyResult.IsFailure)
            {
                return verifyResult.Error;
            }

            var salt = encryptService.GenerateSalt();
            var password = encryptService.Encrypt(request.Password, salt);
            user.UpdatePassword(Password.Create(password), Salt.Create(salt));
            await context.SaveChangesAsync(cancellationToken);
            var token = tokenService.GenerateToken(user);
            return (user, token);
        }
    }

}