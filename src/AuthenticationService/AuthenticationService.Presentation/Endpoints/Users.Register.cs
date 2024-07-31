using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Presentation.Endpoints;

public static partial class Users
{
    public static class Register
    {
        public const string Endpoint = "users/register";

        public record Request(string Username, string Email, string Password);

        public static async Task<IResult> Handler(
            [FromServices]ISender sender, 
            [FromBody]Request request)
        {
            var command = new Application.Users.Commands.Register.Command(
                request.Username,
                request.Email,
                request.Password
            );

            var result = await sender.Send(command);

            return result.IsSuccess ? Results.Created() : result.Error.Problem();
        }
    }

}