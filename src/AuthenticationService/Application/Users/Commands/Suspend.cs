using Domain.Persistence.Contexts;
using Domain.UserAggregate;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Shared.Results;

namespace Application.Users.Commands;

public static class Suspend
{
    public sealed record Command(Guid UserId) : IRequest<Result>;
    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }

    public sealed class Handler(AuthenticationDbContext context) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if (user is null)
            {
                return Errors.Users.NotFound;
            }

            user.Suspend();
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}