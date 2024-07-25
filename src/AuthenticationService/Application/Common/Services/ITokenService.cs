using Domain.UserAggregate;

namespace Application.Common.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}
