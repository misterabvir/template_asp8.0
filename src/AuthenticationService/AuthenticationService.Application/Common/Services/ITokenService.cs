using AuthenticationService.Domain.UserAggregate;

namespace AuthenticationService.Application.Common.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}
