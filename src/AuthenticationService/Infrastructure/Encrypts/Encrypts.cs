using Application.Common.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Encrypts
{
    public static IServiceCollection AddEncrypts(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(Settings.SectionName).Get<Settings>() ??
             throw new Exception("Encrypts settings not configured");
        services.AddSingleton(settings);
        services.AddScoped<IEncryptService, Service>();
        return services;
    }
    
    public class Settings
    {
        public const string SectionName = "Settings:Encrypts";
        public KeyDerivationPrf KeyDerivationPrf { get; set; }
        public int IterationCount { get; set; }
        public int LengthInBytes { get; set; }
    }
    
    public sealed class Service(Settings settings) : IEncryptService
    {
        public byte[] Encrypt(string password, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                password: password, 
                salt: salt, 
                prf: settings.KeyDerivationPrf, 
                iterationCount: settings.IterationCount, 
                numBytesRequested: settings.LengthInBytes);
        }

        public byte[] GenerateSalt()
        {
            return Guid.NewGuid().ToByteArray();
        }
    }
}