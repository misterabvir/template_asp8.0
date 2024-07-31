using System.Security.Claims;

using Shared.Results;

namespace AuthenticationService.Presentation.Endpoints;

public static partial class Users
{
    internal static IResult Problem(this Error error)
    {
        return Results.Problem(
            title: error.Code.ToString(),
            statusCode: (int)error.Code,
            detail: error.Description
        );
    }


    internal static Result<Guid> GetCurrentUserId(this HttpContext context)
    {
        var userStringId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        return string.IsNullOrEmpty(userStringId) || !Guid.TryParse(userStringId, out var userId)
            ? (Result<Guid>)Error.Unauthorized("Not authorized")
            : (Result<Guid>)userId;
    }


    public static class Responses
    {


        public record AuthenticationResult
        {
            public required User User { get; set; }
            public required string Token { get; set; }

            public static AuthenticationResult FromResult((Domain.UserAggregate.User user, string token) value)
            {
                return new AuthenticationResult
                {
                    User = User.FromDomain(value.user),
                    Token = value.token
                };
            }
        }

        public record User
        {
            public required string Status { get; set; }
            public required string Role { get; set; }
            public required DateTime CreatedAt { get; set; }
            public required DateTime UpdatedAt { get; set; }
            public required Data Data { get; set; }
            public required Profile Profile { get; set; }

            public static User FromDomain(Domain.UserAggregate.User user)
            {
                return new User
                {
                    Status = user.Status.Value,
                    Role = user.Role.Value,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    Data = new Data
                    {
                        Username = user.Data.Username,
                        Email = user.Data.Email
                    },
                    Profile = new Profile
                    {
                        FirstName = user.Profile.FirstName.Value,
                        LastName = user.Profile.LastName.Value,
                        ProfilePicture = user.Profile.ProfilePicture.Value,
                        CoverPicture = user.Profile.CoverPicture.Value,
                        Bio = user.Profile.Bio.Value,
                        Gender = user.Profile.Gender.Value,
                        Birthday = user.Profile.Birthday.Value,
                        Website = user.Profile.Website.Value,
                        Location = user.Profile.Location.Value
                    }
                };
            }
        }

        public record Data
        {
            public required string Username { get; set; }
            public required string Email { get; set; }
        }

        public record Profile
        {
            public required string FirstName { get; init; }
            public required string LastName { get; init; }
            public required string ProfilePicture { get; init; }
            public required string CoverPicture { get; init; }
            public required string Bio { get; init; }
            public required string Gender { get; init; }
            public required DateOnly Birthday { get; init; }
            public required string Website { get; init; }
            public required string Location { get; init; }
        }


    }
}

