using AuthenticationService.Application.Common.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
namespace AuthenticationService.Infrastructure;

public static partial class Encrypts
{   
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