using AuthenticationService.Application.Users.Events;

using FluentAssertions;

using AuthenticationService.Infrastructure;

using AuthenticationService.InfrastructureTests.Constants;

using MediatR;

using Microsoft.Extensions.Caching.Distributed;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Domain.Results;

namespace AuthenticationService.InfrastructureTests;

public class VerificationTests
{
    private readonly Verifications.Settings _settings;
    private readonly IDistributedCache _cache;
    private readonly IPublisher _publisher;
    private readonly Verifications.Service _service;

    public VerificationTests()
    {
        _settings = new Verifications.Settings()
        {
            CodeExpirationMinutes = 10,
            TimeOutExpirationInSeconds = 10
        };
        _cache = Substitute.For<IDistributedCache>();
        _publisher = Substitute.For<IPublisher>();
        _service = new Verifications.Service(_settings, _cache, _publisher);
    }

    [Fact]
    public async Task SendVerificationCodeAsync_ShouldTooManyRequestsError_IfTimeOutKeyExistsInCache()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var username = "XXXXXXXX";
        var email = "XXXXXXXX@XXXXX.com";
        var cacheKey = Verifications.Settings.GetVerificationCodeKey(userId);
        var cachedValue = "true";
        var cachedValueBytes = System.Text.Encoding.UTF8.GetBytes(cachedValue);
        _cache.GetAsync(cacheKey, CancellationToken.None)!.Returns(Task.FromResult(cachedValueBytes));

        // Act
        var result = await _service.SendVerificationCodeAsync(userId, username, email, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Verifications.Errors.TooManyRequests);
    }



    [Fact]
    public async Task SendVerificationCodeAsync_ShouldBeReturnSuccessAndSetCodeInCache()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var username = "XXXXXXXX";
        var email = "XXXXXXXX@XXXXX.com";
        var cacheKey = Verifications.Settings.GetTimeOutKey(userId);
        var verificationCodeKey = Verifications.Settings.GetVerificationCodeKey(userId);
        _cache.GetAsync(cacheKey, CancellationToken.None)!.ReturnsNull();

        // Act
        var result = await _service.SendVerificationCodeAsync(userId, username, email, CancellationToken.None);

        // Assert
        await _cache.Received(Receive.Once).GetAsync(cacheKey, CancellationToken.None);
        await _cache.Received(Receive.Twice).SetAsync(Arg.Any<string>(), Arg.Any<byte[]>(), Arg.Any<DistributedCacheEntryOptions>(), Arg.Any<CancellationToken>());
        await _publisher.Received(Receive.Once).Publish(Arg.Any<VerificationCodeSent.Notification>(), Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
    }

    [Fact]
    public async Task VerifyAsync_ShouldReturnCodeExpiredError_IfCodeIsExpired()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var code = "123456";
        var verificationCodeKey = Verifications.Settings.GetVerificationCodeKey(userId);
        var codeBytes = System.Text.Encoding.UTF8.GetBytes(code);
        _cache.GetAsync(verificationCodeKey, CancellationToken.None)!.ReturnsNull();

        // Act
        var result = await _service.VerifyAsync(userId, code, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Verifications.Errors.CodeExpired);
    }

    [Fact]
    public async Task VerifyAsync_ShouldReturnInvalidCodeError_IfCodeIsInvalid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var code = "123456";
        var verificationCodeKey = Verifications.Settings.GetVerificationCodeKey(userId);
        var cachedBytes = System.Text.Encoding.UTF8.GetBytes("987654");
        _cache.GetAsync(verificationCodeKey, CancellationToken.None)!.Returns(cachedBytes);

        // Act
        var result = await _service.VerifyAsync(userId, code, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Verifications.Errors.NotVerified);
    }

    [Fact]
    public async Task VerifyAsync_ShouldReturnSuccessAndDeleteCodeFromCache_IfCodeIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var code = "123456";
        var verificationCodeKey = Verifications.Settings.GetVerificationCodeKey(userId);
        var cachedBytes = System.Text.Encoding.UTF8.GetBytes(code);
        _cache.GetAsync(verificationCodeKey, CancellationToken.None)!.Returns(cachedBytes);

        // Act
        var result = await _service.VerifyAsync(userId, code, CancellationToken.None);

        // Assert
        await _cache.Received(Receive.Twice).RemoveAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
    }
}
