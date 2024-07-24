using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

using MediatR;

using Shared.Results;

namespace Presentation.Endpoints;

public static partial class Users
{
    public static class UpdateData
    {
        public const string Endpoint = "users/update-data";

        public record Request(string Email, string Username);

        public static async Task<IResult> Handler(HttpContext context, ISender sender, Request request)
        {
            var userStringId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userStringId) || !Guid.TryParse(userStringId, out var userId))
            {
                return Responses.Problem(Error.Forbidden("Not authorized"));
            }

            var command = new Application.Users.Commands.UpdateData.Command(userId, request.Username, request.Email);
            var result = await sender.Send(command);
            return result.IsSuccess ? Results.Ok() : Responses.Problem(result.Error);
        }
    }
}