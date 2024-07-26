using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Presentation.Endpoints;

public static partial class Users
{
    public static partial class ResetPassword
    {
        public const string Endpoint = "users/reset-password";
        public record Request(string Email, string Password, string Code);

        public static async Task<IResult> Handler([FromServices]ISender sender, [FromBody]Request request){
            var command = new Application.Users.Commands.ResetPassword.Command(request.Email, request.Password, request.Code);
            var result = await sender.Send(command);
            return result.IsSuccess ? Results.Ok(Responses.AuthenticationResult.FromResult(result.Value)) : result.Error.Problem();
        }
    }
}

