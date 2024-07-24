using Application.Common.Services;

using Domain.Persistence.Contexts;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Shared.Results;

namespace Application.Users.Commands;

public static class UpdatePassword
{
    public sealed record Command(Guid UserId, string Password) : IRequest<Result>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().Matches(Password.Regex);
        }
    }

    public sealed class Handler(AuthenticationDbContext context, IEncryptService encryptService) : IRequestHandler<Command, Result>
    {

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if(user is null){
                return Errors.Users.NotFound;
            }

            var salt = encryptService.GenerateSalt();
            var password = encryptService.Encrypt(request.Password, salt);

            user.UpdatePassword(Password.Create(password), Salt.Create(salt));

            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}