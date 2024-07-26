using Application.Common.Services;

using Domain.Persistence.Contexts;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Shared.Results;

namespace Application.Users.Commands;

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

    public sealed class Handler(AuthenticationDbContext context, ITokenService tokenService) : IRequestHandler<Command, Result<(User user, string Token)>>
    {
        public async Task<Result<(User user, string Token)>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await context.Users.AnyAsync(u => u.Data.Username == request.Username, cancellationToken))
            {
                return Errors.Users.UsernameAlreadyTaken;
            }

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if(user is null){
                return Errors.Users.NotFound;
            }
            
            user.UpdateUsername(Username.Create(request.Username));
            await context.SaveChangesAsync(cancellationToken);
            var token = tokenService.GenerateToken(user);
            return (user, token);
        }
    }
}
