using AuthenticationService.Application.Common.Repositories;
using AuthenticationService.Domain.UserAggregate;
using AuthenticationService.Domain.UserAggregate.ValueObjects;

using Domain.Abstractions;
using Domain.Results;

using FluentValidation;
using MediatR;

namespace AuthenticationService.Application.Users.Commands;

public static class ChangeRole
{
    public sealed record Command(Guid InitiatorUserId, Guid TargetUserId, string RoleName) : IRequest<Result>;
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.InitiatorUserId).NotEmpty().NotEqual(x => x.TargetUserId);
            RuleFor(c => c.TargetUserId).NotEmpty();
            RuleFor(c => c.RoleName).NotEmpty().Must(AuthorizationConstants.AllRoles.Contains);
        }
    }

    public sealed class Handler(IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var initiator = await unitOfWork.Users.GetByIdAsync(request.InitiatorUserId, cancellationToken);
            if (initiator is null)
            {
                return Errors.Users.NotFound;
            }

            if (initiator.Role != Role.Administrator)
            {
                return Errors.Users.NotHavePermission;
            }

            var target = await unitOfWork.Users.GetByIdAsync(request.TargetUserId, cancellationToken);
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

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}