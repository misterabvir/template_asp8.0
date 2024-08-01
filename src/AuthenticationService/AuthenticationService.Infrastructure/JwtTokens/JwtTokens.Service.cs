using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AuthenticationService.Application.Common.Services;
using AuthenticationService.Domain.UserAggregate;
using Infrastructure;

namespace AuthenticationService.Infrastructure;

public static partial class JwtTokens
{
    public sealed class Service(Tokens.Settings settings) : ITokenService
    {
        public string GenerateToken(User user)
        {
            var expiration = DateTime.Now.AddMinutes(settings.ExpirationMinutes);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.Value.ToString()),
                new Claim(ClaimTypes.Email, user.Data.Email.Value),
                new Claim(ClaimTypes.Role, user.Role.Value.ToString()),
                new Claim(ClaimTypes.Name, user.Data.Username.Value.ToString()),
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



