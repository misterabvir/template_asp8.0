using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

using MediatR;

using Shared.Results;

namespace Presentation.Endpoints;

public static partial class Users
{
    public static class UpdateUsername
    {
        public const string Endpoint = "users/update-username";

        public record Request(string Username);

        public static async Task<IResult> Handler(HttpContext context, ISender sender, Request request)
        {
            var resultCurrentUserId = context.GetCurrentUserId();
            if (resultCurrentUserId.IsFailure){
                return resultCurrentUserId.Error.Problem();
            }

            var command = new Application.Users.Commands.UpdateUsername.Command(resultCurrentUserId.Value, request.Username);
            var result = await sender.Send(command);
            return result.IsSuccess ? Results.Ok() : result.Error.Problem();
        }
    }
}