using Application.Common.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;


public static partial class EmailSender
{
    public static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(Settings.SectionName).Get<Settings>() ??
            throw new EmailSettingsNotConfiguredException();
        services.AddSingleton(settings);
        services.AddScoped<IEmailSender, Service>();
        return services;
    }
}