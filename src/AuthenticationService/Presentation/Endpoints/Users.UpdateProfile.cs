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
             var resultCurrentUserId = context.GetCurrentUserId();
            if (resultCurrentUserId.IsFailure){
                return resultCurrentUserId.Error.Problem();
            }

            var command = new Application.Users.Commands.UpdateProfile.Command(
                resultCurrentUserId.Value, 
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