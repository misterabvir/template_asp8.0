using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Domain.Persistence.Contexts;
using Domain.Persistence.Interceptors;

namespace Domain.Persistence;

public static class DependencyInjection
{
    public const string DbConnectionString = "DbConnection";
    
    /// <summary>
    /// Registration of the persistence for domain entities.
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