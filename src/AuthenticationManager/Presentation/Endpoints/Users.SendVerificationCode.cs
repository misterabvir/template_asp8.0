using MediatR;

namespace Presentation.Endpoints;

public static partial class Users
{
    public static partial class SendVerificationCode
    {
        public const string Endpoint = "users/send-verification-code";
        public record Request(string Email);

        public static async Task<IResult> Handler(ISender sender, Request request)
        {
            var query = new Application.Users.Queries.SendVerificationCode.Query(request.Email);
            var result = await sender.Send(query);         
            return result.IsSuccess ? Results.Created() : Responses.Problem(result.Error);
        }
    }
}

