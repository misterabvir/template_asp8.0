using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Shared.Results;

namespace Presentation.Endpoints;

public static partial class Users
{
    public static class Suspend
    {
        public const string Endpoint = "users/suspend";

        public record Request();

        public static async Task<IResult> Handler(HttpContext context, [FromServices]ISender sender, [FromBody]Request request)
        {
            var userStringId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userStringId) || !Guid.TryParse(userStringId, out var userId))
            {
                return Error.Unauthorized("Not authorized").Problem();
            }
            var command = new Application.Users.Commands.Suspend.Command(userId);     
            var result = await sender.Send(command);
            return result.IsSuccess ? Results.Ok() : result.Error.Problem();
        }
    }
}