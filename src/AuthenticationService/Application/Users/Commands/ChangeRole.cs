using Domain.Persistence.Contexts;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Shared.Domain;
using Shared.Results;

namespace Application.Users.Commands;

public static class ChangeRole
{
    public sealed record Command(Guid InitiatorUserId, Guid TargetUserId, string RoleName) : IRequest<Result>;
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.InitiatorUserId).NotEmpty().NotEqual(x => x.TargetUserId);
            RuleFor(c => c.TargetUserId).NotEmpty();
            RuleFor(c => c.RoleName).NotEmpty().Must(AppPermissions.Roles.Contains);
        }
    }

    public sealed class Handler(AuthenticationDbContext context) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var initiator = await context.Users.FirstOrDefaultAsync(u => u.Id == request.InitiatorUserId, cancellationToken);
            if (initiator is null)
            {
                return Errors.Users.NotFound;
            }

            if (initiator.Role != Role.Administrator)
            {
                return Errors.Users.NotHavePermission;
            }

            var target = await context.Users.FirstOrDefaultAsync(u => u.Id == request.TargetUserId, cancellationToken);
            if (target is null)
            {
                return Errors.Users.NotFound;
            }

            var role = Role.Create(request.RoleName);

            if (target.Role == role)
            {
                return Errors.Users.AlreadyHaveThisRole;
            }

            target.ChangeRole(role);

            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}