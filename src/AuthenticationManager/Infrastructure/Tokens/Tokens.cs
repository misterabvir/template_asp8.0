using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Application.Common.Services;

using Domain.UserAggregate;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Shared.Domain;

namespace Infrastructure;

public static class Tokens
{
    public static IServiceCollection AddTokens(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration.GetSection(Settings.SectionName).Get<Settings>() ??
            throw new Exception("Token settings not configured.");
        services.AddSingleton(settings);
        services.AddScoped<ITokenService, Service>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = settings.TokenValidationParameters);
        services.AddAuthorizationBuilder()
            .AddPolicy(AppPermissions.AdministratorPolicy, policy => policy.RequireRole(AppPermissions.AdministratorRole))
            .AddPolicy(AppPermissions.UserPolicy, policy => policy.RequireRole(AppPermissions.Roles));    
        return services;
    }

    public sealed class Settings
    {
        public const string SectionName = "Settings:Tokens";
        public string Secret { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int ExpirationMinutes { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }

        public SymmetricSecurityKey SymmetricSecurityKey => new(Encoding.UTF8.GetBytes(Secret));
        public SigningCredentials SigningCredentials => new(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        public TokenValidationParameters TokenValidationParameters => new()
        {
            ValidateIssuer = ValidateIssuer,
            ValidateAudience = ValidateAudience,
            ValidateLifetime = ValidateLifetime,
            ValidateIssuerSigningKey = ValidateIssuerSigningKey,
            ValidIssuer = Issuer,
            ValidAudience = Audience,
            IssuerSigningKey = SymmetricSecurityKey
        };
    }

    public sealed class Service(Settings settings) : ITokenService
    {
        public string GenerateToken(User user)
        {
            var expiration = DateTime.Now.AddMinutes(settings.ExpirationMinutes);    
            var claims = new []
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Data.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Name, user.Data.Username.ToString()),
                new Claim(ClaimTypes.GivenName, $"{user.Profile.FirstName.Value} {user.Profile.LastName.Value}"),
                new Claim(ClaimTypes.DateOfBirth, user.Profile.Birthday.Value.ToShortDateString()),
                new Claim(ClaimTypes.Gender, user.Profile.Gender.Value),
                new Claim(ClaimTypes.Country, user.Profile.Location.Value),
                new Claim(ClaimTypes.Expiration, expiration.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: settings.Issuer,
                audience: settings.Audience,
                claims: claims,
                expires: expiration,
                signingCredentials: settings.SigningCredentials
            );

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }
    }
}