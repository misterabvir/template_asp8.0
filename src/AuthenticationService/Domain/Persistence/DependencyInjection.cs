using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Domain.Persistence.Contexts;
using Domain.Persistence.Interceptors;

using Quartz;

namespace Domain.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionDbString = configuration.GetConnectionString("DbConnection");
        services.AddDbContext<AuthenticationDbContext>(options =>
        {
            options.UseNpgsql(connectionDbString);
            options.UseSnakeCaseNamingConvention();
            options.AddInterceptors(new ConvertDomainEventsToOutboxMessagesInterceptor());
        });

        

        return services;
    }

}