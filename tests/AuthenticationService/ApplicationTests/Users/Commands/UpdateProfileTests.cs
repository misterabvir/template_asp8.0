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

public class UpdateProfileTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly UpdateProfile.Handler _handler;

    public UpdateProfileTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _tokenService = Substitute.For<ITokenService>();
        _handler = new UpdateProfile.Handler(_unitOfWork, _tokenService);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnNotFoundError_IfUserNotExists()
    {
        // Arrange
        var command = GetCommand(Guid.NewGuid());
        _unitOfWork.Users.GetByIdAsync(command.UserId, CancellationToken.None).ReturnsNull();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByIdAsync(command.UserId, CancellationToken.None);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Users.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldBeChangedProfileAndReturnSuccess()
    {
        // Arrange
        var token = "expected_token";
        var user = User.Create(
            Username.Create("XXXXX"),
            Email.Create("XXXXX"),
            Password.Create([]),
            Salt.Create([]));
        var command = GetCommand(user.Id.Value);
        _unitOfWork.Users.GetByIdAsync(command.UserId, CancellationToken.None).Returns(user);
        _tokenService.GenerateToken(user).Returns(token);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);    

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByIdAsync(command.UserId, CancellationToken.None);
        _tokenService.Received(Receive.Once).GenerateToken(user);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be((user, token));
        result.Value.User.Profile.Birthday.Value.Should().Be(command.Birthday);
        result.Value.User.Profile.FirstName.Value.Should().Be(command.FirstName);
        result.Value.User.Profile.LastName.Value.Should().Be(command.LastName);
        result.Value.User.Profile.ProfilePicture.Value.Should().Be(command.ProfilePicture);
        result.Value.User.Profile.CoverPicture.Value.Should().Be(command.CoverPicture);
        result.Value.User.Profile.Bio.Value.Should().Be(command.Bio);
        result.Value.User.Profile.Gender.Value.Should().Be(command.Gender);
        result.Value.User.Profile.Website.Value.Should().Be(command.Website);
        result.Value.User.Profile.Location.Value.Should().Be(command.Location);
        result.Value.Token.Should().Be(token);
    }


    private static UpdateProfile.Command GetCommand(Guid userId)
    {
        return new UpdateProfile.Command(
             UserId: userId,
             Birthday: DateOnly.FromDateTime(DateTime.Parse("2000-01-01")),
             FirstName: "NewFirstName",
             LastName: "NewLastName",
             ProfilePicture: "NewProfilePicture",
             CoverPicture: "NewCoverPicture",
             Bio: "NewBio",
             Gender: Gender.Male.Value,
             Website: "NewWebsite",
             Location: "NewLocation");
    }
}
