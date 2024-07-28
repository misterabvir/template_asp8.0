using MediatR;

using Microsoft.AspNetCore.Mvc;
namespace Presentation.Endpoints;

public static partial class Users
{
    public static class Login
    {
        public const string Endpoint = "users/login";
        public record Request(string Email, string Password);

        public static async Task<IResult> Handler(
            [FromServices] ISender sender, 
            [FromBody] Request request)
        {
            var query = new Application.Users.Queries.Login.Query(request.Email, request.Password);
            var result = await sender.Send(query);
            return result.IsSuccess ? Results.Ok(Responses.AuthenticationResult.FromResult(result.Value)) : result.Error.Problem();
        }
    }
}