using Application.Common.Repositories;
using Application.Common.Services;
using Application.Users.Commands;
using ApplicationTests.Constants;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ApplicationTests.Users.Commands;

public class UpdatePasswordTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEncryptService _encryptService;
    private readonly UpdatePassword.Handler _handler;

    public UpdatePasswordTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _encryptService = Substitute.For<IEncryptService>();
        _handler = new UpdatePassword.Handler(_unitOfWork, _encryptService);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnUserNotFound_IdUserNotExists()
    {
        // Arrange
        var command = new UpdatePassword.Command(Guid.NewGuid(), "XXXXXXXX");
        _unitOfWork.Users.GetByIdAsync(command.UserId, CancellationToken.None).ReturnsNull();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByIdAsync(command.UserId, Arg.Any<CancellationToken>());
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Users.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldBeChangePassword()
    {
        // Arrange
        byte[] oldPassword = [1, 1, 1];
        byte[] oldSalt= [2, 2, 2];
        byte[] newPassword = [11, 11, 11];
        byte[] newSalt = [22, 22, 22]; 

        var user = User.Create(
                Username.Create("XXXXXXXX"),
                Email.Create("XXXXXXXX"),
                Password.Create(oldPassword),   
                Salt.Create(oldSalt)   
            );

        var command = new UpdatePassword.Command(user.Id, "XXXXXXXX");
        _unitOfWork.Users.GetByIdAsync(command.UserId, CancellationToken.None).Returns(user);
        _encryptService.GenerateSalt().Returns(newSalt);
        _encryptService.Encrypt(command.Password, newSalt).Returns(newPassword);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByIdAsync(command.UserId, Arg.Any<CancellationToken>());
        _encryptService.Received(Receive.Once).GenerateSalt();
        _encryptService.Received(Receive.Once).Encrypt(command.Password, newSalt);
        await _unitOfWork.Received(Receive.Once).SaveChangesAsync(Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
        user.Data.Password.Value.Should().BeEquivalentTo(newPassword);
        user.Data.Salt.Value.Should().BeEquivalentTo(newSalt);
    }

}
