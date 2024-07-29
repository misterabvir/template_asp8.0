using Application.Common.Repositories;
using Application.Common.Services;
using Application.Users.Commands;

using ApplicationTests.Constants;

using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using FluentAssertions;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Shared.Results;

namespace ApplicationTests.Users.Commands;

public class RestoreTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVerificationService _verificationService;
    private readonly IEncryptService _encryptService;
    private readonly ITokenService _tokenService;
    private readonly Restore.Handler _handler;

    public RestoreTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _verificationService = Substitute.For<IVerificationService>();
        _encryptService = Substitute.For<IEncryptService>();
        _tokenService = Substitute.For<ITokenService>();
        _handler = new Restore.Handler(_unitOfWork, _verificationService, _encryptService, _tokenService);    
    }

    [Fact]
    public async Task Handle_ShouldBeReturnNotFoundError_IfUserNotExists()
    {
        // Arrange
        var command = new Restore.Command("test@example.com", "test_password", "test_code");
        _unitOfWork.Users.GetByEmailAsync(command.Email, CancellationToken.None).ReturnsNull();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByEmailAsync(command.Email, CancellationToken.None);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Users.NotFound);
    }

    [Fact]

    public async Task Handle_ShouldBeReturnVerifyFailError_IfCodeNotVerified()
    {
        // Arrange
        var email = "test-email";
        var user = User.Create(
            Username.Create("user"),
            Email.Create(email),
            Password.Create([]),
            Salt.Create([]));
        var verifyFail = Error.Forbidden("Verify fail");
        var command = new Restore.Command(email, "password", "test-code");
        _unitOfWork.Users.GetByEmailAsync(command.Email, CancellationToken.None).Returns(user);
        _verificationService.VerifyAsync(user.Id, command.VerificationCode, CancellationToken.None).Returns(verifyFail);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByEmailAsync(command.Email, CancellationToken.None);
        await _verificationService.Received(Receive.Once).VerifyAsync(user.Id, command.VerificationCode, CancellationToken.None);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(verifyFail);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnSuccess_IfDataIsValid()
    {
        // Arrange
        var token = "token";
        byte[] salt = []; 
        byte[] password = []; 
        var email = "test-email";
        var user = User.Create(
            Username.Create("user"),
            Email.Create(email),
            Password.Create([]),
            Salt.Create([]));
        var command = new Restore.Command(email, "test-password", "test-code");
        _unitOfWork.Users.GetByEmailAsync(command.Email, CancellationToken.None).Returns(user);
        _verificationService.VerifyAsync(user.Id, command.VerificationCode, CancellationToken.None).Returns(Result.Success());
        _encryptService.GenerateSalt().Returns(salt);
        _encryptService.Encrypt(command.Password, salt).Returns(password);
        _tokenService.GenerateToken(user).Returns(token);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByEmailAsync(command.Email, CancellationToken.None);
        await _verificationService.Received(Receive.Once).VerifyAsync(user.Id, command.VerificationCode, CancellationToken.None);
        _encryptService.Received(Receive.Once).GenerateSalt();
        _encryptService.Received(Receive.Once).Encrypt(command.Password, salt);
        _tokenService.Received(Receive.Once).GenerateToken(user);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be((user, token));
    }
}
