using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MailService.Infrastructure;

public static partial class Tokens
{
    public const string AdminRole = "Administrator";
    public const string AdminPolicy = "AdministratorPolicy";

    public static IServiceCollection AddTokenAuthorization(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        var settings = configuration.GetSection(Settings.SectionName).Get<Settings>() ?? throw new TokenSettingsNotConfiguredException();
        services.AddSingleton(settings);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => options.TokenValidationParameters = settings.TokenValidationParameters);
        services.AddAuthorizationBuilder().AddPolicy(AdminPolicy, policy => policy.RequireRole(AdminRole));
        return services;
    }

}

