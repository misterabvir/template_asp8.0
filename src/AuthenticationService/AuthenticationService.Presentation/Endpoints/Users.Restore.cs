using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Presentation.Endpoints;

public static partial class Users
{
    public static class Restore
    {
        public const string Endpoint = "users/restore";

        public record Request(string Email, string Password, string VerificationCode);

        public static async Task<IResult> Handler(
            [FromServices] ISender sender,
            [FromBody] Request request)
        {
            var command = new Application.Users.Commands.Restore.Command(request.Email, request.Password, request.VerificationCode);
            var result = await sender.Send(command);
            return result.IsSuccess ? Results.Ok() : result.Error.Problem();
        }
    }
}