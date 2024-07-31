using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Presentation.Endpoints;

public static partial class Users
{
    public static class Suspend
    {
        public const string Endpoint = "users/suspend";

        public record Request();

        public static async Task<IResult> Handler(HttpContext context, [FromServices]ISender sender, [FromBody]Request request)
        {
             var resultCurrentUserId = context.GetCurrentUserId();
            if (resultCurrentUserId.IsFailure){
                return resultCurrentUserId.Error.Problem();
            }

            var command = new Application.Users.Commands.Suspend.Command(resultCurrentUserId.Value);     
            var result = await sender.Send(command);
            return result.IsSuccess ? Results.Ok() : result.Error.Problem();
        }
    }
}