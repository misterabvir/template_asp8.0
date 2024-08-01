using Domain.Abstractions;

namespace MailService.Presentation.Endpoints;

public static partial class Messages
{
    public static WebApplication MapMessagesEndpoints(this WebApplication app)
    {
        app.MapGet(GetLastEmails.Route, GetLastEmails.Handler).RequireAuthorization(AuthorizationConstants.Policies.Administrator);
        app.MapGet(GetLastUsersEmails.Route, GetLastUsersEmails.Handler).RequireAuthorization(AuthorizationConstants.Policies.Administrator);
        app.MapGet("/test-auth", () => "authorized").RequireAuthorization();
        return app;
    }
}

