using AuthenticationService.Application.Common.Services;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Infrastructure;


public static partial class JwtTokens
{
    public static IServiceCollection AddJwtTokens(this IServiceCollection services)
    {
        services.AddTokenAuthorization();
        services.AddScoped<ITokenService, Service>();
        return services;
    }

}