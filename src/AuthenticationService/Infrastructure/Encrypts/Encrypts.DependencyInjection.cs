using Application.Common.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static partial class Encrypts
{
    public static IServiceCollection AddEncrypts(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(Settings.SectionName).Get<Settings>() ??
             throw new EncryptsSettingsNotConfiguredException();
        services.AddSingleton(settings);
        services.AddScoped<IEncryptService, Service>();
        return services;
    }
}