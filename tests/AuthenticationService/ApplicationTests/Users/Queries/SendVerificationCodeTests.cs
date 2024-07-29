using Application.Common.Repositories;
using Application.Common.Services;
using Application.Users.Queries;

using ApplicationTests.Constants;

using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using FluentAssertions;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Shared.Results;

namespace ApplicationTests.Users.Queries;

public class SendVerificationCodeTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVerificationService _verificationService;
    private readonly SendVerificationCode.Handler _handler;

    public SendVerificationCodeTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _verificationService = Substitute.For<IVerificationService>();
        _handler = new SendVerificationCode.Handler(_unitOfWork, _verificationService);
    }


    [Fact]
    public async Task Handle_ShouldBeReturnNotFoundError_IfUserNotExists()
    {
        // Arrange
        var query = new SendVerificationCode.Query("test@example.com");
        _unitOfWork.Users.GetByEmailAsync(query.Email, CancellationToken.None).ReturnsNull();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByEmailAsync(query.Email, CancellationToken.None);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Users.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnManyRequestsError_IfDelayNotDone()
    {
        // Arrange
        var query = new SendVerificationCode.Query("test@example.com");
        var user = User.Create(
            Username.Create("test"),
            Email.Create(query.Email),
            Password.Create([]),
            Salt.Create([]));
        var manyRequestsFail = Error.Conflict("Many requests");
        _unitOfWork.Users.GetByEmailAsync(query.Email, CancellationToken.None).Returns(user);
        _verificationService.SendVerificationCodeAsync(user.Id, user.Data.Username, user.Data.Email, CancellationToken.None)
            .Returns(manyRequestsFail);
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByEmailAsync(query.Email, CancellationToken.None);
        await _verificationService.Received(Receive.Once).SendVerificationCodeAsync(user.Id, user.Data.Username, user.Data.Email, CancellationToken.None);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(manyRequestsFail);
    }

    [Fact]
    public async Task Handle_ShouldBeReturnSuccess()
    {
        // Arrange
        var query = new SendVerificationCode.Query("test@example.com");
        var user = User.Create(
            Username.Create("test"),
            Email.Create(query.Email),
            Password.Create([]),
            Salt.Create([]));
        _unitOfWork.Users.GetByEmailAsync(query.Email, CancellationToken.None).Returns(user);
        _verificationService.SendVerificationCodeAsync(user.Id, user.Data.Username, user.Data.Email, CancellationToken.None)
            .Returns(Result.Success());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _unitOfWork.Users.Received(Receive.Once).GetByEmailAsync(query.Email, CancellationToken.None);
        await _verificationService.Received(Receive.Once).SendVerificationCodeAsync(user.Id, user.Data.Username, user.Data.Email, CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
    }
}
