using AuthenticationService.Application.Common.Repositories;
using AuthenticationService.Application.Common.Services;
using AuthenticationService.Application.Users.Commands;
using AuthenticationService.Domain.UserAggregate;
using AuthenticationService.Domain.UserAggregate.ValueObjects;

using FluentAssertions;

using AuthenticationService.ApplicationTests.Constants;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace AuthenticationService.ApplicationTests.Users.Commands;

public class UpdateUsernameTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;

    private readonly UpdateUsername.Handler _handler;

    public UpdateUsernameTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _tokenService = Substitute.For<ITokenService>();
        _handler = new UpdateUsername.Handler(_unitOfWork, _tokenService);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnUsernameAlreadyTakenError_IfUsernameExists()
    {
        // Arrange
        var username = "new_username";
        var command = new UpdateUsername.Command(Guid.NewGuid(), username);
        _unitOfWork.Users.IsUsernameNotUnique(username, CancellationToken.None).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).IsUsernameNotUnique(username, Arg.Any<CancellationToken>());
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Users.UsernameAlreadyTaken);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnNotFoundError_IfUserNotExists()
    {
        // Arrange
        var user = User.Create(
            Username.Create("user"),
            Email.Create("user@example.com"),
            Password.Create([]),
            Salt.Create([]));
        var username = "new_username";
        var command = new UpdateUsername.Command(user.Id, username);
        _unitOfWork.Users.IsUsernameNotUnique(username, CancellationToken.None).Returns(false);
        _unitOfWork.Users.GetByIdAsync(command.UserId, CancellationToken.None).ReturnsNull();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).IsUsernameNotUnique(username, Arg.Any<CancellationToken>());
        await _unitOfWork.Users.Received(Receive.Once).GetByIdAsync(command.UserId, Arg.Any<CancellationToken>());
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Users.NotFound);
    }


    [Fact]
    public async Task Handle_ShouldBeChangeUsernameAndReturnSuccess()
    {
        // Arrange
        var user = User.Create(
            Username.Create("user"),
            Email.Create("user@example.com"),
            Password.Create([]),
            Salt.Create([]));
        var username = "new_username";
        var command = new UpdateUsername.Command(user.Id, username);
        _unitOfWork.Users.IsUsernameNotUnique(username, CancellationToken.None).Returns(false);
        _unitOfWork.Users.GetByIdAsync(command.UserId, CancellationToken.None).Returns(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).IsUsernameNotUnique(username, Arg.Any<CancellationToken>());
        await _unitOfWork.Users.Received(Receive.Once).GetByIdAsync(command.UserId, Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
        user.Data.Username.Value.Should().Be(username);
    }
}


