using Application.Common.Repositories;
using Application.Common.Services;
using Application.Users.Queries;

using ApplicationTests.Constants;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using FluentAssertions;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ApplicationTests.Users.Queries;

public class LoginTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IEncryptService _encryptService;
    private readonly Login.Handler _handler;

    public LoginTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _tokenService = Substitute.For<ITokenService>();
        _encryptService = Substitute.For<IEncryptService>();
        _handler = new Login.Handler(_unitOfWork, _tokenService, _encryptService);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnInvalidCredentialsError_IfUserNotExists()
    {
        // Arrange
        var query = new Login.Query("test@example.com", "test-password");
        _unitOfWork.Users.GetByEmailAsync(query.Email, CancellationToken.None).ReturnsNull();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByEmailAsync(query.Email, CancellationToken.None);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Users.InvalidCredentials);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnInvalidCredentialsError_IfUserStatusNotActive()
    {
        // Arrange
        var query = new Login.Query("test@example.com", "test-password");
        var user = User.Create(
            Username.Create("test"),
            Email.Create(query.Email),
            Password.Create([]),
            Salt.Create([]));
        _unitOfWork.Users.GetByEmailAsync(query.Email, CancellationToken.None).Returns(user);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByEmailAsync(query.Email, CancellationToken.None);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Users.InvalidCredentials);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnInvalidCredentialsError_IfUserPasswordNotSame()
    {
        // Arrange
        var query = new Login.Query("test@example.com", "test-password");
        var user = User.Create(
            Username.Create("test"),
            Email.Create(query.Email),
            Password.Create([]),
            Salt.Create([]));
        user.Verify();
        byte[] hash = [1, 2, 3];
        _unitOfWork.Users.GetByEmailAsync(query.Email, CancellationToken.None).Returns(user);
        _encryptService.Encrypt(query.Password, user.Data.Salt.Value).Returns(hash);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByEmailAsync(query.Email, CancellationToken.None);
        _encryptService.Received(Receive.Once).Encrypt(query.Password, user.Data.Salt.Value);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Users.InvalidCredentials);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnToken()
    {
        // Arrange
        var token = "token";
        var query = new Login.Query("test@example.com", "test-password");
        var user = User.Create(
            Username.Create("test"),
            Email.Create(query.Email),
            Password.Create([]),
            Salt.Create([]));
        user.Verify();
        _unitOfWork.Users.GetByEmailAsync(query.Email, CancellationToken.None).Returns(user);
        _encryptService.Encrypt(query.Password, user.Data.Salt.Value).Returns(user.Data.Password.Value);
        _tokenService.GenerateToken(user).Returns(token);  
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByEmailAsync(query.Email, CancellationToken.None);
        _encryptService.Received(Receive.Once).Encrypt(query.Password, user.Data.Salt.Value);
        _tokenService.Received(Receive.Once).GenerateToken(user);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be((user, token));        
    }
}
