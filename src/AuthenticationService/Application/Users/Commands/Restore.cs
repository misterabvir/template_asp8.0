using Application.Common.Repositories;
using Application.Common.Services;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using FluentValidation;
using MediatR;
using Shared.Results;

namespace Application.Users.Commands;

public static class Restore
{
    public record Command(string Email, string Password, string VerificationCode) : IRequest<Result<(User User, string Token)>>;
    public class Validator : AbstractValidator<Command>
    {
        public Validator(IVerificationService verificationService)
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().Matches(Email.Regex);
            RuleFor(x => x.Password).NotEmpty().Matches(Password.Regex);
            RuleFor(x => x.VerificationCode).Length(verificationService.VerificationCodeLength);
        }
    }  
    
    public class Handler(
        IUnitOfWork unitOfWork,
        IVerificationService verificationService,
        IEncryptService encryptService,
        ITokenService tokenService
        ) : IRequestHandler<Command, Result<(User User, string Token)>>
    {

       
        public async Task<Result<(User User, string Token)>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);
            if (user is null)
            {
                return Errors.Users.NotFound;
            }

            var result = await verificationService.VerifyAsync(user.Id, request.VerificationCode, cancellationToken);
            if (result.IsFailure)
            {
                return result.Error;
            }

            user.Verify();
            
            var salt = encryptService.GenerateSalt();
            var password = encryptService.Encrypt(request.Password, salt);
            user.UpdatePassword(Password.Create(password), Salt.Create(salt));

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var token = tokenService.GenerateToken(user);

            return (user, token);
        }
    }   

}
