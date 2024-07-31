using AuthenticationService.Application.Common.Repositories;
using AuthenticationService.Application.Common.Services;
using AuthenticationService.Domain.UserAggregate;
using AuthenticationService.Domain.UserAggregate.ValueObjects;
using FluentValidation;
using MediatR;
using Shared.Results;

namespace AuthenticationService.Application.Users.Commands;

public static class UpdateUsername
{
    public sealed record Command(Guid UserId, string Username) :
    IRequest<Result<(User user, string Token)>>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.Username).NotEmpty().Matches(Username.Regex);
        }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, ITokenService tokenService) : IRequestHandler<Command, Result<(User user, string Token)>>
    {
        public async Task<Result<(User user, string Token)>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await unitOfWork.Users.IsUsernameNotUnique(request.Username, cancellationToken))
            {
                return Errors.Users.UsernameAlreadyTaken;
            }

            var user = await unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
            if(user is null){
                return Errors.Users.NotFound;
            }
            
            user.UpdateUsername(Username.Create(request.Username));
            await unitOfWork.SaveChangesAsync(cancellationToken);
            var token = tokenService.GenerateToken(user);
            return (user, token);
        }
    }
}
