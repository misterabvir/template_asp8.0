using System.Security.Claims;
using MassTransit;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using MailService.Presentation.Common.Exceptions;
using MailService.Presentation.Common.Settings;
using MailService.Presentation.Consumers;
using MailService.Presentation.Endpoints;
using MailService.Presentation.Common;

namespace MailService.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var tokenSettings = configuration.GetSection(TokenSettings.SectionName).Get<TokenSettings>() ?? throw new TokenSettingsNotConfiguredException();        
        services.AddSingleton(tokenSettings);
        services.AddConsumers(configuration);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => options.TokenValidationParameters = tokenSettings.TokenValidationParameters);
        services.AddAuthorizationBuilder().AddPolicy(Constants.AdminPolicy, policy => policy.RequireRole(Constants.AdminRole));
        return services;
    }

    public static WebApplication UsePresentation(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapMessagesEndpoints();
        return app;
    }
}
