using MediatR;

namespace Presentation.Endpoints;

public static partial class Users
{
    public static partial class VerifyCode
    {
        public const string Endpoint = "users/verify-code";
        public record Request(string Email, string Code);

        public static async Task<IResult> Handler(ISender sender, Request request)
        {
            var command = new Application.Users.Commands.VerifyCode.Command(request.Email, request.Code);
            var result = await sender.Send(command);
            return result.IsSuccess ? Results.Ok(Responses.AuthenticationResult.FromResult(result.Value)) : Responses.Problem(result.Error);
        }
    }
}