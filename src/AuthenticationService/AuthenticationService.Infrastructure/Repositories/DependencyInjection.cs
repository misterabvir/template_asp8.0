using AuthenticationService.Application.Common.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
