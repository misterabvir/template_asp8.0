using MailService.Infrastructure;

namespace MailService.Presentation.Endpoints;

public static partial class Messages
{
    public static WebApplication MapMessagesEndpoints(this WebApplication app)
    {
        app.MapGet(GetLastEmails.Route, GetLastEmails.Handler).RequireAuthorization(Tokens.AdminPolicy);
        app.MapGet(GetLastUsersEmails.Route, GetLastUsersEmails.Handler).RequireAuthorization(Tokens.AdminPolicy);
        return app;
    }
}
