using Application.Common.Repositories;
using Application.Users.Commands;

using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using FluentAssertions;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ApplicationTests.Users.Commands;

public class SuspendTests
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly Suspend.Handler _handler;

    public SuspendTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new Suspend.Handler(_unitOfWork);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnNotFoundError_IdUserNotExists()
    {
        // Arrange     
        var command = new Suspend.Command(Guid.NewGuid());
        _unitOfWork.Users.GetByIdAsync(command.UserId, default).ReturnsNull();

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Users.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldSuspendUserAccountAndReturnSuccess()
    {
        // Arrange
        var user = User.Create(
            Username.Create("John"), 
            Email.Create("john@example.com"), 
            Password.Create([]), 
            Salt.Create([]));
        var command = new Suspend.Command(user.Id);
        _unitOfWork.Users.GetByIdAsync(command.UserId, default).Returns(user);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        user.Status.Should().Be(Status.Suspended);
    }
}
