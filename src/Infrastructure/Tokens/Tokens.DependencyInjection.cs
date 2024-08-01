using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Abstractions;
namespace Infrastructure;

public static partial class Tokens
{
    public static IServiceCollection AddTokenAuthorization(
        this IServiceCollection services)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("Tokens/token-settings.json")
            .Build();

        var settings = configuration.GetSection(Settings.SectionName).Get<Settings>() ?? throw new TokenSettingsNotConfiguredException();
        services.AddSingleton(settings);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => options.TokenValidationParameters = settings.TokenValidationParameters);
        services.AddAuthorizationBuilder() 
            .AddPolicy(AuthorizationConstants.Policies.Administrator, policy => policy.RequireRole(AuthorizationConstants.Roles.Administrator))
            .AddPolicy(AuthorizationConstants.Policies.User, policy => policy.RequireRole(AuthorizationConstants.AllRoles));    
        return services;
    }

}

