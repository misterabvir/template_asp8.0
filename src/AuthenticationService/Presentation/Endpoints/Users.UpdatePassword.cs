using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

using MediatR;

using Shared.Results;

namespace Presentation.Endpoints;

public static partial class Users
{
    public static class UpdatePassword
    {

        public const string Endpoint = "users/update-password";

        public record Request(string Password);

        public static async Task<IResult> Handler(HttpContext context, ISender sender, Request request)
        {
            var userStringId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userStringId) || !Guid.TryParse(userStringId, out var userId))
            {
                return Error.Forbidden("Not authorized").Problem();
            }

            var command = new Application.Users.Commands.UpdatePassword.Command(userId, request.Password);
            var result = await sender.Send(command);
            return result.IsSuccess ? Results.Ok() : result.Error.Problem();
        }
    }
}