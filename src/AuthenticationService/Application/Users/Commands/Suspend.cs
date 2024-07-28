using Application.Common.Repositories;
using Domain.UserAggregate;
using FluentValidation;
using MediatR;
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

    public sealed class Handler(IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
            {
                return Errors.Users.NotFound;
            }

            user.Suspend();
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}