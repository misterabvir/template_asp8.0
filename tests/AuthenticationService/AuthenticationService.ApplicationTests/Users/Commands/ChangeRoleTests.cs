using AuthenticationService.Application.Common.Repositories;
using AuthenticationService.Application.Users.Commands;
using AuthenticationService.ApplicationTests.Constants;
using AuthenticationService.Domain.UserAggregate;
using AuthenticationService.Domain.UserAggregate.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shared.Results;

namespace AuthenticationService.ApplicationTests.Users.Commands;

public class ChangeRoleTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ChangeRole.Handler _handler;

    public ChangeRoleTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new ChangeRole.Handler(_unitOfWork);
    }


    [Fact]
    public async Task Handle_ShouldBeReturnNotFoundError_IfInitiatorNotExist()
    {
        var initiatorId = Guid.NewGuid();   
        var targetId = Guid.NewGuid();
        var role = Role.Administrator.Value;
        
        var command = new ChangeRole.Command(initiatorId, targetId, role);
        _unitOfWork.Users.GetByIdAsync(initiatorId).ReturnsNull();

        var result = await _handler.Handle(command, CancellationToken.None);

        await _unitOfWork.Users.Received(Receive.Once).GetByIdAsync(initiatorId);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(Code.NotFound);
    }


    [Fact]
    public async Task Handle_ShouldBeReturnForbiddenError_IfInitiatorNotAdmin()
    {
        // Arrange
        var initiatorId = Guid.NewGuid();
        var targetId = Guid.NewGuid();
        var role = Role.Administrator.Value;
        var initiator = User.Create(
                Username.Create("initiator"),
                Email.Create("initiator@example.com"),
                Password.Create([]),
                Salt.Create([]));           
        var command = new ChangeRole.Command(initiatorId, targetId, role);
        _unitOfWork.Users.GetByIdAsync(initiatorId).Returns(initiator);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByIdAsync(initiatorId);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Users.NotHavePermission);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnNotFoundError_IfTargetUserNotExist()
    {
        // Arrange
        var initiatorId = Guid.NewGuid();
        var targetId = Guid.NewGuid();
        var role = Role.Administrator.Value;
        var initiator = User.Create(
            Username.Create("initiator"),
            Email.Create("initiator@example.com"),
            Password.Create([]),
            Salt.Create([]));
        initiator.ChangeRole(Role.Administrator);
        var command = new ChangeRole.Command(initiatorId, targetId, role);
        _unitOfWork.Users.GetByIdAsync(initiatorId).Returns(initiator);
        _unitOfWork.Users.GetByIdAsync(targetId).ReturnsNull();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Arrange
        await _unitOfWork.Users.Received(Receive.Twice).GetByIdAsync(Arg.Any<Guid>());
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Users.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnAlreadyHaveThisRoleError_IfTargetUserAlreadyHasRole()
    {
        // Arrange
        var initiatorId = Guid.NewGuid();
        var targetId = Guid.NewGuid();
        var role = Role.Administrator.Value;
        var initiator = User.Create(
            Username.Create("initiator"),
            Email.Create("initiator@example.com"),
            Password.Create([]),
            Salt.Create([]));
        initiator.ChangeRole(Role.Administrator);
        var target = User.Create(
            Username.Create("target"),
            Email.Create("target@example.com"),
            Password.Create([]),
            Salt.Create([]));
        target.ChangeRole(Role.Administrator);
        var command = new ChangeRole.Command(initiatorId, targetId, role);
        _unitOfWork.Users.GetByIdAsync(initiatorId).Returns(initiator);
        _unitOfWork.Users.GetByIdAsync(targetId).Returns(target);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Twice).GetByIdAsync(Arg.Any<Guid>());
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Users.AlreadyHaveThisRole);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnSuccess()
    {
        // Arrange
        var initiatorId = Guid.NewGuid();
        var targetId = Guid.NewGuid();
        var role = Role.Administrator.Value;
        var initiator = User.Create(
            Username.Create("initiator"),
            Email.Create("initiator@example.com"),
            Password.Create([]),
            Salt.Create([]));
        initiator.ChangeRole(Role.Administrator);
        var target = User.Create(
            Username.Create("target"),
            Email.Create("target@example.com"),
            Password.Create([]),
            Salt.Create([]));
        var command = new ChangeRole.Command(initiatorId, targetId, role);
        _unitOfWork.Users.GetByIdAsync(initiatorId).Returns(initiator);
        _unitOfWork.Users.GetByIdAsync(targetId).Returns(target);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(2).GetByIdAsync(Arg.Any<Guid>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
    }
}
