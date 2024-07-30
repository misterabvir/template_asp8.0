using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using FluentAssertions;

using Infrastructure;

namespace InfrastructureTests;

public class TokenTests
{
    private readonly Tokens.Settings _settings;
    private readonly Tokens.Service _service;

    public TokenTests()
    {
        _settings = new Tokens.Settings()
        {
            Audience = "test-Audience",
            Issuer = "test-Issuer",
            Secret = "test-SecretKeyForTestingTokenServiceForGeneratedJwtTokens",
            ExpirationMinutes = 10
        };
        _service = new Tokens.Service(_settings);
    }

    [Fact]
    public void GenerateToken_ReturnsToken()
    {
        // Arrange
        var user = User.Create(
            Username.Create("test-username"),
            Email.Create("test-email@example.com"),
            Password.Create([]),
            Salt.Create([]));
        user.UpdateProfile(
             FirstName.Create("test-firstName"),
             LastName.Create("test-lastName"),
             ProfilePicture.Empty,
             CoverPicture.Empty,
             Bio.Empty,
             Gender.Male,
             Birthday.Create(DateOnly.FromDateTime(DateTime.Parse("2000-01-01"))),
             Website.Empty,
             Location.Empty);

        // Act
        var token = _service.GenerateToken(user);

        //Arrange
        var handler = new JwtSecurityTokenHandler();

        handler.CanReadToken(token).Should().BeTrue();
        var validation = handler.ValidateToken(token, _settings.TokenValidationParameters, out var validatedToken);       
        validation.Should().NotBeNull();
        validatedToken.Should().NotBeNull();
        validatedToken.Issuer.Should().Be(_settings.Issuer);
        validation.Identity.Should().NotBeNull();
        validation.Identity!.IsAuthenticated.Should().BeTrue();
        validation.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == user.Id.Value.ToString());
        validation.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == user.Data.Username.Value);
        validation.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == user.Data.Email.Value);
        validation.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == user.Role.Value);
        validation.Claims.Should().Contain(c => c.Type == ClaimTypes.GivenName && c.Value == $"{user.Profile.FirstName.Value} {user.Profile.LastName.Value}");
        validation.Claims.Should().Contain(c => c.Type == ClaimTypes.DateOfBirth && c.Value == user.Profile.Birthday.Value.ToString());
        validation.Claims.Should().Contain(c => c.Type == ClaimTypes.Gender && c.Value == user.Profile.Gender.Value);
        validation.Claims.Should().Contain(c => c.Type == ClaimTypes.Country && c.Value == user.Profile.Location.Value);
    }
}
