using System.Reflection;

using DbUp;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MailService.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
       this IServiceCollection services,
       IConfiguration configuration)
    {

        var connectionDbString = configuration.GetConnectionString("DbConnection") ??
            throw new Exception("DbConnectionString is not configured");
        services.AddSingleton(p => new DbConnectionFactory(connectionDbString));
        return services;
    }



}