using AuthenticationService.Application.Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Infrastructure;
public static partial class Verifications
{
   
    public static IServiceCollection AddVerifications(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var settings = configuration.GetSection(Settings.SectionName).Get<Settings>()??
            throw new VerifySettingsNotConfiguredException();
        services.AddSingleton(settings);       
        return services.AddScoped<IVerificationService, Service>();
    }
}