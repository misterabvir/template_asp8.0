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
            var resultCurrentUserId = context.GetCurrentUserId();
            if (resultCurrentUserId.IsFailure){
                return resultCurrentUserId.Error.Problem();
            }


            var command = new Application.Users.Commands.UpdatePassword.Command(resultCurrentUserId.Value, request.Password);
            var result = await sender.Send(command);
            return result.IsSuccess ? Results.Ok() : result.Error.Problem();
        }
    }
}