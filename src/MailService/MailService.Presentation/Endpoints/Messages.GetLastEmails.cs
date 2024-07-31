using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MailService.Presentation.Endpoints;

public static partial class Messages
{
    public static class GetLastEmails
    {


        public const string Route = "/last-emails/{amount}";
        private const int DefaultAmount = 10;
        public static async Task<IResult> Handler([FromServices] ISender sender, [FromQuery] int amount = DefaultAmount)
        {
            {
                if (amount <= 0) return Results.BadRequest();
                var query = new MailService.Application.Queries.GetLastEmails.Query(amount);
                var result = await sender.Send(query);
                return Results.Ok(result.ToResponse());
            };
        }

    }
}


