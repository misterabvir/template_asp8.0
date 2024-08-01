using System.Security.Cryptography;
using System.Text;

using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static partial class Tokens
{

    public sealed class Settings
    {
        public const string SectionName = "Settings:Tokens";
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int ExpirationMinutes { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }

        private RsaSecurityKey? _publicKey;
        private RsaSecurityKey? _privateKey;
        public RsaSecurityKey PublicKey
        {
            get
            {
                if (_publicKey is null)
                {
                    var rsa = RSA.Create();
                    rsa.ImportFromPem(File.ReadAllText(AppContext.BaseDirectory + "/Tokens/rsa/public_key.pem"));
                    _publicKey = new RsaSecurityKey(rsa);
                }
                return _publicKey;
            }
        }

        public RsaSecurityKey PrivateKey
        {
            get
            {
                if (_privateKey is null)
                {
                    var rsa = RSA.Create();
                    rsa.ImportFromPem(File.ReadAllText(AppContext.BaseDirectory + "/Tokens/rsa/private_key.pem"));
                    _privateKey = new RsaSecurityKey(rsa);
                }
                return _privateKey;
            }
        }
        public SigningCredentials SigningCredentials => new (PrivateKey, SecurityAlgorithms.RsaSha256Signature);

        public TokenValidationParameters TokenValidationParameters => new()
        {
            ValidateIssuer = ValidateIssuer,
            ValidateAudience = ValidateAudience,
            ValidateLifetime = ValidateLifetime,
            ValidateIssuerSigningKey = ValidateIssuerSigningKey,
            ValidIssuer = Issuer,
            ValidAudience = Audience,
            IssuerSigningKey = PublicKey
        };
    }
}

