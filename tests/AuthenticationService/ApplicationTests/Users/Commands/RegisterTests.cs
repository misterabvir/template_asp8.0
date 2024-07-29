using Application.Common.Repositories;
using Application.Common.Services;
using Application.Users.Commands;
using ApplicationTests.Constants;
using Domain.UserAggregate;
using FluentAssertions;
using NSubstitute;

namespace ApplicationTests.Users.Commands;

public class RegisterTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEncryptService _encryptService;
    private readonly Register.Handler _handler;

    public RegisterTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _encryptService = Substitute.For<IEncryptService>();
        _handler = new Register.Handler(_unitOfWork, _encryptService);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnUsernameAlreadyTakenError_IfUsernameExists()
    {
        // Arrange
        var username = "username";
        var email = "email";
        var password = "XXXXXXXX";  
        var command = new Register.Command(username, email, password);
        _unitOfWork.Users.IsUsernameNotUnique(username, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        await _unitOfWork.Users.Received(Receive.Once).IsUsernameNotUnique(username, Arg.Any<CancellationToken>());    
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Users.UsernameAlreadyTaken);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnEmailAlreadyTakenError_IfEmailExists()
    {
        // Arrange
        var username = "username";
        var email = "email";
        var password = "XXXXXXXX";

        var command = new Register.Command(username, email, password);
        _unitOfWork.Users.IsUsernameNotUnique(username, Arg.Any<CancellationToken>()).Returns(false);
        _unitOfWork.Users.IsEmailNotUnique(email, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).IsUsernameNotUnique(username, Arg.Any<CancellationToken>());
        await _unitOfWork.Users.Received(Receive.Once).IsEmailNotUnique(email, Arg.Any<CancellationToken>());
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Users.EmailAlreadyTaken);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnSuccess()
    {
        // Arrange
        var username = "username";
        var email = "email";
        var password = "XXXXXXXX";
        var salt = new byte[] {1,2,3};
        var hashedPassword = new byte[] {4,5,6};

        var command = new Register.Command(username, email, password);
        _unitOfWork.Users.IsUsernameNotUnique(username, Arg.Any<CancellationToken>()).Returns(false);
        _unitOfWork.Users.IsEmailNotUnique(email, Arg.Any<CancellationToken>()).Returns(false);
        _encryptService.GenerateSalt().Returns(salt);
        _encryptService.Encrypt(password, salt).Returns(hashedPassword);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).IsUsernameNotUnique(username, Arg.Any<CancellationToken>());
        await _unitOfWork.Users.Received(Receive.Once).IsEmailNotUnique(email, Arg.Any<CancellationToken>());
        _encryptService.Received(Receive.Once).GenerateSalt();
        _encryptService.Received(Receive.Once).Encrypt(password, salt);
        await _unitOfWork.Users.Received(1).AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
    }
}
