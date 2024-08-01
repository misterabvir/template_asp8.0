using System.IdentityModel.Tokens.Jwt;
using Application.Events;
using MailService.Presentation.Common;
using MailService.Presentation.Common.Settings;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
