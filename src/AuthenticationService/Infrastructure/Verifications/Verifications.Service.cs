using Application.Common.Services;

using Microsoft.Extensions.Caching.Distributed;
using Shared.Results;

namespace Infrastructure;
public static partial class Verifications
{
  
    public sealed class Service(Settings settings, IDistributedCache cache, IPublisher publisher) : IVerificationService
    {
        public int VerificationCodeLength => settings.VerificationCodeLength;

        public async Task<Result> SendVerificationCodeAsync(Guid userId, string username, string email, CancellationToken cancellationToken)
        {
            var cachedTimeOut = await cache.GetStringAsync(Settings.GetTimeOutKey(userId), token: cancellationToken);
            if (cachedTimeOut is not null){
                return Error.BadRequest("To many requests, try again later");
            }
            
            var code = settings.GetRandomCode();
            await cache.SetStringAsync(Settings.GetVerificationCodeKey(userId), code, settings.CodeExpirationOptions, cancellationToken);
            await cache.SetStringAsync(Settings.GetTimeOutKey(userId), "true", settings.TimeOutOptions, cancellationToken);
            
            await publisher.Publish(new Application.Users.Events.VerificationCodeSent.Notification(username, email, code), cancellationToken);

            return Result.Success();
        }

        public async Task<Result> VerifyAsync(Guid userId, string code, CancellationToken cancellationToken)
        {
            var cachedCode = await cache.GetStringAsync(Settings.GetVerificationCodeKey(userId), token: cancellationToken);
            if (cachedCode is null){
                return Error.BadRequest("Verification code expired");
            }
            if (cachedCode != code){
                return Error.BadRequest("Invalid verification code");
            }
            await cache.RemoveAsync(Settings.GetVerificationCodeKey(userId), cancellationToken);
            await cache.RemoveAsync(Settings.GetTimeOutKey(userId), cancellationToken);
            return Result.Success();
        }
    }
}