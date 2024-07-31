using AuthenticationService.Application.Common.Repositories;
using AuthenticationService.Application.Common.Services;
using AuthenticationService.Domain.UserAggregate;
using AuthenticationService.Domain.UserAggregate.ValueObjects;
using FluentValidation;
using MediatR;
using Domain.Results;

namespace AuthenticationService.Application.Users.Commands;

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
        IUnitOfWork unitOfWork, 
        ITokenService tokenService, 
        IVerificationService verificationService) : 
        IRequestHandler<Command, Result<(User User, string Token)>>
    {
        public async Task<Result<(User User, string Token)>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);
            if (user is null)
            {
                return Errors.Users.NotFound;
            }

            if (user.Status == Status.Active)
            {
                return Errors.Users.AlreadyActive;
            }

            var result = await verificationService.VerifyAsync(user.Id, request.Code, cancellationToken);
            if (result.IsFailure)
            {
                return result.Error;
            }

            user.Verify();
            await unitOfWork.SaveChangesAsync(cancellationToken);
            var token = tokenService.GenerateToken(user);
            return (user, token);
        }
    }
}