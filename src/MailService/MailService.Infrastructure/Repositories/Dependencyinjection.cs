using MailService.Application.Common.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MailService.Infrastructure.Repositories;

public static class DependencyInjection
{
     public static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<TemplateRepository>();

        return services;
    }
}