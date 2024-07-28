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

public static partial class Tokens
{
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