using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MailService.Presentation.Endpoints;

public static partial class Messages
{
    public static class GetLastUsersEmails
    {
        public const string Route = "/last-user-emails/{amount}/{userId}";
        private const int DefaultAmount = 10;
        public static async Task<IResult> Handler([FromServices] ISender sender, [FromQuery] Guid userId, [FromQuery] int amount = DefaultAmount)
        {

            if (amount <= 0) return Results.BadRequest();
            var query = new MailService.Application.Queries.GetLastUserEmails.Query(userId, amount);
            var result = await sender.Send(query);
            return Results.Ok(result.ToResponse());
        }
    }
}
