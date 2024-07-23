using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Persistence.Contexts;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionDbString = configuration.GetConnectionString("DbConnection");
        services.AddDbContext<AuthenticationDbContext>(options =>
        {
            options.UseSqlServer(connectionDbString);
            options.UseSnakeCaseNamingConvention();
        });

        return services;
    }

}