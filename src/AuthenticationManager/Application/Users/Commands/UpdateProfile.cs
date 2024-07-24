using Application.Common.Services;

using Domain.Persistence.Contexts;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Shared.Results;

namespace Application.Users.Commands;

public static class UpdateProfile
{
    public sealed record Command(
        Guid UserId,
        DateOnly Birthday,
        string FirstName = "",
        string LastName = "",
        string ProfilePicture = "",
        string CoverPicture = "",
        string Bio = "",
        string Gender = Gender.None,
        string Website = "",
        string Location = Location.None
    ) : IRequest<Result<(User User, string Token)>>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.UserId).NotEmpty();
        }
    }

    public sealed class Handler(AuthenticationDbContext context, ITokenService tokenService) : IRequestHandler<Command, Result<(User User, string Token)>>
    {
        public async Task<Result<(User User, string Token)>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if(user is null)
            {
                return Errors.Users.NotFound;
            }

            user.UpdateProfile(
                FirstName.Create(request.FirstName),
                LastName.Create(request.LastName),
                ProfilePicture.Create(request.ProfilePicture),
                CoverPicture.Create(request.CoverPicture),
                Bio.Create(request.Bio),
                Gender.Create(request.Gender),
                Birthday.Create(request.Birthday),
                Website.Create(request.Website),
                Location.Create(request.Location)
            );

            await context.SaveChangesAsync(cancellationToken);

            var token = tokenService.GenerateToken(user);
            return (user, token);
        }
    }
}