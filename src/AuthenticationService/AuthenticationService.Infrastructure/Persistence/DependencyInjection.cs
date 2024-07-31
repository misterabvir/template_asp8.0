using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using AuthenticationService.Infrastructure.Persistence.Contexts;
using AuthenticationService.Infrastructure.Persistence.Interceptors;

namespace AuthenticationService.Infrastructure.Persistence;

public static class DependencyInjection
{
    public const string DbConnectionString = "DbConnection";
    
    /// <summary>
    /// Registration of the persistence for AuthenticationService.Domain entities.
    /// Required configured connection string in appsettings.json
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionDbString = configuration.GetConnectionString(DbConnectionString)
        ?? throw new DbConnectionNotConfiguredException();
        services.AddDbContext<AuthenticationDbContext>(options =>
        {
            options.UseNpgsql(connectionDbString);
            options.UseSnakeCaseNamingConvention();
            options.AddInterceptors(new ConvertDomainEventsToOutboxMessagesInterceptor());
        });
        return services;
    }

    
    public class DbConnectionNotConfiguredException : Exception
    {
        public DbConnectionNotConfiguredException() : base("DbConnection not configured")
        {
        }
    }
}