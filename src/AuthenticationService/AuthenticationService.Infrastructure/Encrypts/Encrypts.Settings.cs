using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace AuthenticationService.Infrastructure;

public static partial class Encrypts
{

    public class Settings
    {
        public const string SectionName = "Settings:Encrypts";
        public KeyDerivationPrf KeyDerivationPrf { get; set; }
        public int IterationCount { get; set; }
        public int LengthInBytes { get; set; }
    }
}