using Application.Common.Services;

using Domain.Persistence.Contexts;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Shared.Results;

namespace Application.Users.Commands;

public static class VerifyCode
{
    public record Command(string Email, string Code) : IRequest<Result<(User User, string Token)>>;
    public class Validator : AbstractValidator<Command>
    {
        public Validator(IVerificationService verificationService)
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().Matches(Email.Regex);
            RuleFor(x => x.Code).NotEmpty().Length(verificationService.VerificationCodeLength);
        }
    }

    public sealed class Handler(
        AuthenticationDbContext context, 
        ITokenService tokenService, 
        IVerificationService verificationService) : 
        IRequestHandler<Command, Result<(User User, string Token)>>
    {
        public async Task<Result<(User User, string Token)>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(u=>u.Data.Email == request.Email, cancellationToken);
            if (user is null)
            {
                return Errors.Users.NotFound;
            }

            if (user.Status == Status.Active)
            {
                return Errors.Users.AlreadyActive;
            }

            var result = await verificationService.VerifyAsync(user, request.Code, cancellationToken);
            if (result.IsFailure)
            {
                return result.Error;
            }

            user.Verify();
            await context.SaveChangesAsync(cancellationToken);
            var token = tokenService.GenerateToken(user);
            return (user, token);
        }
    }
}