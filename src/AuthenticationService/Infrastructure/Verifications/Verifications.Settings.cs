using Microsoft.Extensions.Caching.Distributed;
namespace Infrastructure;
public static partial class Verifications
{
  
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

        public static string GetVerificationCodeKey(Guid userId) => $"VerificationCode:{userId}";
        public static string GetTimeOutKey(Guid userId) => $"TimeOut:{userId}";
    }
}