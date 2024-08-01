using AuthenticationService.Application.Common.Repositories;
using AuthenticationService.Application.Common.Services;
using AuthenticationService.Infrastructure.BackgroundJobs;
using AuthenticationService.Infrastructure.Persistence;
using AuthenticationService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddPersistence(configuration)
            .AddRepositories()
            .AddEncrypts(configuration)
            .AddVerifications(configuration)
            .AddBackgroundJobs(configuration)
            .AddJwtTokens()
            ;

        return services;
    }   
}