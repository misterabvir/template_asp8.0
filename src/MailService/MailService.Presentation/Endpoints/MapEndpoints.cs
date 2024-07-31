using MailService.Presentation.Common;

namespace MailService.Presentation.Endpoints;

public static partial class Messages
{
    public static WebApplication MapMessagesEndpoints(this WebApplication app)
    {
        app.MapGet(GetLastEmails.Route, GetLastEmails.Handler).RequireAuthorization(Constants.AdminPolicy);
        app.MapGet(GetLastUsersEmails.Route, GetLastUsersEmails.Handler).RequireAuthorization(Constants.AdminPolicy);

        return app;
    }
}
