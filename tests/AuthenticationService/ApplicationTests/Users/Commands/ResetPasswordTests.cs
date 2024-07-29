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

public class ResetPasswordTests
{   
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVerificationService _verificationService;
    private readonly IEncryptService _encryptService;
    private readonly ITokenService _tokenService;

    private readonly ResetPassword.Handler _handler;

    public ResetPasswordTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _verificationService = Substitute.For<IVerificationService>();
        _encryptService = Substitute.For<IEncryptService>();
        _tokenService = Substitute.For<ITokenService>();

        _handler = new ResetPassword.Handler(_unitOfWork, _verificationService, _encryptService, _tokenService);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnNotFound_IfUserNotExist()
    {
        // Arrange
        var email = "email";
        var password = "XXXXXXXX";
        var code = "code";
        var command = new ResetPassword.Command(email, password, code);
        _unitOfWork.Users.GetByEmailAsync(email, Arg.Any<CancellationToken>()).ReturnsNull();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByEmailAsync(email, Arg.Any<CancellationToken>());
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Users.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnVerifyError_IfCodeNotVerified()
    {
        // Arrange
        var email = "email";
        var password = "XXXXXXXX";
        var code = "code";
        var error = Error.Forbidden("Verify failed");
        var user = User.Create(Username.Create("username"), Email.Create(email), Password.Create([]), Salt.Create([]));
        var command = new ResetPassword.Command(email, password, code);
        _unitOfWork.Users.GetByEmailAsync(email, Arg.Any<CancellationToken>()).Returns(user);
        _verificationService.VerifyAsync(user.Id, code, Arg.Any<CancellationToken>()).Returns(error);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByEmailAsync(email, Arg.Any<CancellationToken>());
        await _verificationService.Received(Receive.Once).VerifyAsync(user.Id, code, Arg.Any<CancellationToken>());
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnSuccessWithUserAndToken()
    {
        // Arrange
        var email = "email";
        var username = "username";
        var password = "XXXXXXXX";
        var code = "code";
        byte[] oldSalt = [11, 12, 13];
        byte[] oldHashedPassword = [14, 15, 16];
        byte[] newSalt = [1, 2, 3];
        byte[] newHashedPassword = [4, 5, 6];
        var token = "token";
        var user = User.Create(Username.Create(username), Email.Create(email), Password.Create(oldHashedPassword), Salt.Create(oldSalt));
        var command = new ResetPassword.Command(email, password, code);
        
        _unitOfWork.Users.GetByEmailAsync(email, Arg.Any<CancellationToken>()).Returns(user);
        _verificationService.VerifyAsync(user.Id, code, Arg.Any<CancellationToken>()).Returns(Result.Success());
        _encryptService.GenerateSalt().Returns(newSalt);
        _encryptService.Encrypt(password, newSalt).Returns(newHashedPassword);
        _tokenService.GenerateToken(user).Returns(token);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByEmailAsync(email, Arg.Any<CancellationToken>());
        await _verificationService.Received(Receive.Once).VerifyAsync(user.Id, code, Arg.Any<CancellationToken>());
        _encryptService.Received(Receive.Once).GenerateSalt();
        _encryptService.Received(Receive.Once).Encrypt(password, newSalt);
        _tokenService.Received(Receive.Once).GenerateToken(user);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be((user, token));
        result.Value.User.Data.Password.Value.Should().BeEquivalentTo(newHashedPassword);
        result.Value.User.Data.Salt.Value.Should().BeEquivalentTo(newSalt);
        result.Value.Token.Should().Be(token);
    }

}
