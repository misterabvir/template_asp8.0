using AuthenticationService.Application.Common.Services;

using Domain.Abstractions;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;



namespace AuthenticationService.Infrastructure;

public static partial class Tokens
{
    public static IServiceCollection AddTokens(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration.GetSection(Settings.SectionName).Get<Settings>() ??
            throw new TokenSettingsNotConfiguredException();
        services.AddSingleton(settings);
        services.AddScoped<ITokenService, Service>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = settings.TokenValidationParameters);
        services.AddAuthorizationBuilder()
            .AddPolicy(AuthorizationConstants.Policies.Administrator, policy => policy.RequireRole(AuthorizationConstants.Roles.Administrator))
            .AddPolicy(AuthorizationConstants.Policies.User, policy => policy.RequireRole(AuthorizationConstants.AllRoles));    
        return services;
    }
}