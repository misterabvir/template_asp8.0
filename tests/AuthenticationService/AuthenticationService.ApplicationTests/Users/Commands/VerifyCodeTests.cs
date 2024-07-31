using AuthenticationService.Application.Common.Repositories;
using AuthenticationService.Application.Common.Services;
using AuthenticationService.Application.Users.Commands;
using AuthenticationService.ApplicationTests.Constants;
using AuthenticationService.Domain.UserAggregate;
using AuthenticationService.Domain.UserAggregate.ValueObjects;

using FluentAssertions;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Shared.Results;

namespace AuthenticationService.ApplicationTests.Users.Commands;

public class VerifyCodeTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IVerificationService _verificationService;
    private readonly VerifyCode.Handler _handler;

    public VerifyCodeTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _tokenService = Substitute.For<ITokenService>();
        _verificationService = Substitute.For<IVerificationService>();
        _handler = new VerifyCode.Handler(_unitOfWork, _tokenService, _verificationService);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnNotFoundError_IdUserNotExists()
    {
        // Arrange
        var command = new VerifyCode.Command("test-email", "test-code");
        _unitOfWork.Users.GetByEmailAsync(command.Email, CancellationToken.None).ReturnsNull();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Users.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnAlreadyActiveError_IdUserHasActiveStatus()
    {
        // Arrange
        var email = "test-email";
        var user = User.Create(
            Username.Create("user"),
            Email.Create(email),
            Password.Create([]),
            Salt.Create([]));
        user.Verify();
        var command = new VerifyCode.Command(email, "test-code");
        _unitOfWork.Users.GetByEmailAsync(command.Email, CancellationToken.None).Returns(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByEmailAsync(command.Email, CancellationToken.None);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Users.AlreadyActive);
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
        var command = new VerifyCode.Command(email, "test-code");
        _unitOfWork.Users.GetByEmailAsync(command.Email, CancellationToken.None).Returns(user);
        _verificationService.VerifyAsync(user.Id, command.Code, CancellationToken.None).Returns(verifyFail);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByEmailAsync(command.Email, CancellationToken.None);
        await _verificationService.Received(Receive.Once).VerifyAsync(user.Id, command.Code, CancellationToken.None);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(verifyFail);
    }

    [Fact]
    public async Task Handle_ShouldBeChangeUserStatusOnActiveAndReturnSuccess()
    {
        // Arrange
        var token = "test-token";
        var email = "test-email";
        var user = User.Create(
            Username.Create("user"),
            Email.Create(email),
            Password.Create([]),
            Salt.Create([]));
        var command = new VerifyCode.Command(email, "test-code");
        _unitOfWork.Users.GetByEmailAsync(command.Email, CancellationToken.None).Returns(user);
        _verificationService.VerifyAsync(user.Id, command.Code, CancellationToken.None).Returns(Result.Success());
        _tokenService.GenerateToken(user).Returns(token);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByEmailAsync(command.Email, CancellationToken.None);
        await _verificationService.Received(Receive.Once).VerifyAsync(user.Id, command.Code, CancellationToken.None);
        _tokenService.Received(Receive.Once).GenerateToken(user);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be((user, token));
        result.Value.User.Status.Should().Be(Status.Active);
        result.Value.Token.Should().Be(token);  
    }

}
