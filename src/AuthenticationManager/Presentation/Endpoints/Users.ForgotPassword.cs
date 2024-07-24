using MediatR;

namespace Presentation.Endpoints;

public static partial class Users
{
    public static class ForgotPassword
    {

        public const string Endpoint = "users/forgot-password";
       
        public record Request(string Email);

        public static async Task<IResult> Handler(ISender sender, Request request)
        {
            var query = new Application.Users.Queries.ForgotPassword.Query(request.Email);
            var result = await sender.Send(query);
            return result.IsSuccess ? Results.Ok() : Responses.Problem(result.Error);     
        }
    }
}