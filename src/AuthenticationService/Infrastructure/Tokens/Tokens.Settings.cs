using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static partial class Tokens
{    public sealed class Settings
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

}