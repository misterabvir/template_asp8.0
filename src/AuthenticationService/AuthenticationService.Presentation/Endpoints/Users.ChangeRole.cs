using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace AuthenticationService.Presentation.Endpoints;

public static partial class Users
{
    public static class ChangeRole
    {
        public const string Endpoint = "users/change-role";

        public record Request(Guid TargetId, string RoleName);

        public static async Task<IResult> Handler(
            [FromServices] HttpContext context,
            [FromServices] ISender sender,
            [FromBody] Request request)
        {
            var resultCurrentUserId = context.GetCurrentUserId();
            if (resultCurrentUserId.IsFailure)
            {
                return resultCurrentUserId.Error.Problem();
            }


            var command = new Application.Users.Commands.ChangeRole.Command(resultCurrentUserId.Value, request.TargetId, request.RoleName);
            var result = await sender.Send(command);
            return result.IsSuccess ? Results.Ok() : result.Error.Problem();
        }
    }
}