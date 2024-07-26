using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

using MediatR;

using Shared.Results;

namespace Presentation.Endpoints;

public static partial class Users
{
    public static class UpdateProfile
    {

        public const string Endpoint = "users/update-profile";

        public record Request(
            DateOnly? Birthday = null,
            string? FirstName = null,
            string? LastName = null,
            string? ProfilePicture = null,
            string? CoverPicture = null,
            string? Bio = null,
            string? Gender = null,
            string? Website = null,
            string? Location = null
        );

        public static async Task<IResult> Handler(HttpContext context, ISender sender, Request request)
        {
            var userStringId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userStringId) || !Guid.TryParse(userStringId, out var userId))
            {
                return Error.Unauthorized("Not authorized").Problem();
            }
            var command = new Application.Users.Commands.UpdateProfile.Command(
                userId, 
                request.Birthday ?? DateOnly.MinValue, 
                request.FirstName ?? string.Empty, 
                request.LastName ?? string.Empty, 
                request.ProfilePicture ?? string.Empty,
                request.CoverPicture ?? string.Empty, 
                request.Bio ?? string.Empty, 
                request.Gender ?? string.Empty, 
                request.Website ?? string.Empty, 
                request.Location ?? string.Empty);      
            var result = await sender.Send(command);
            return result.IsSuccess ? Results.Ok() : result.Error.Problem();
        }
    }
}