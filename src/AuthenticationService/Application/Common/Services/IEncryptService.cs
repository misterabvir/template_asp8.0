using Domain.UserAggregate.ValueObjects;

namespace Application.Common.Services;

public interface IEncryptService
{
    byte[] Encrypt(string password, byte[] salt);
    byte[] GenerateSalt();
}
