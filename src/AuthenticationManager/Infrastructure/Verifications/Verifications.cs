using Application.Common.Services;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using MediatR;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Results;

namespace Infrastructure;

public static class Verifications
{
   
    public static IServiceCollection AddVerifications(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var settings = configuration.GetSection(Settings.SectionName).Get<Settings>()??
            throw new Exception("Verification settings not configured");
        services.AddSingleton(settings);       
        return services.AddScoped<IVerificationService, Service>();
    }
   
    public class Settings{
        public const string SectionName = "Settings:Verification";
        public int VerificationCodeLength { get; set; }
        public int TimeOutExpirationInSeconds { get; set; }
        public int CodeExpirationMinutes { get; set; }

        public DistributedCacheEntryOptions TimeOutOptions => new ()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(TimeOutExpirationInSeconds)
        };
        public DistributedCacheEntryOptions CodeExpirationOptions => new ()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CodeExpirationMinutes)
        };

        public string GetRandomCode() => Guid.NewGuid().ToString()[..VerificationCodeLength];

        public static string GetVerificationCodeKey(UserId userId) => $"VerificationCode:{userId.Value}";
        public static string GetTimeOutKey(UserId userId) => $"TimeOut:{userId.Value}";
    }
    
   
    public sealed class Service(Settings settings, IDistributedCache cache, IPublisher publisher) : IVerificationService
    {
        public int VerificationCodeLength => settings.VerificationCodeLength;

        public async Task<Result> SendVerificationCodeAsync(User user, CancellationToken cancellationToken)
        {
            var cachedTimeOut = await cache.GetStringAsync(Settings.GetTimeOutKey(user.Id), token: cancellationToken);
            if (cachedTimeOut is not null){
                return Error.BadRequest("To many requests, try again later");
            }
            
            var code = settings.GetRandomCode();
            await cache.SetStringAsync(Settings.GetVerificationCodeKey(user.Id), code, settings.CodeExpirationOptions, cancellationToken);
            await cache.SetStringAsync(Settings.GetTimeOutKey(user.Id), "true", settings.TimeOutOptions, cancellationToken);
            
            await publisher.Publish(new Application.Users.Events.VerificationCodeSent.Notification(user, code), cancellationToken);

            return Result.Success();
        }

        public async Task<Result> VerifyAsync(User user, string code, CancellationToken cancellationToken)
        {
            var cachedCode = await cache.GetStringAsync(Settings.GetVerificationCodeKey(user.Id), token: cancellationToken);
            if (cachedCode is null){
                return Error.BadRequest("Verification code expired");
            }
            if (cachedCode != code){
                return Error.BadRequest("Invalid verification code");
            }
            await cache.RemoveAsync(Settings.GetVerificationCodeKey(user.Id), cancellationToken);
            await cache.RemoveAsync(Settings.GetTimeOutKey(user.Id), cancellationToken);
            return Result.Success();
        }
    }
}