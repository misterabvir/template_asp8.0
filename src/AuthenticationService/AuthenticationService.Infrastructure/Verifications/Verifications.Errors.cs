using Microsoft.Extensions.Caching.Distributed;

using Shared.Results;
namespace AuthenticationService.Infrastructure;
public static partial class Verifications
{
    public static class Errors
    {
        public static readonly Error NotVerified = Error.BadRequest("Invalid verification code");
        public static readonly Error CodeExpired = Error.BadRequest("Code expired");    
        public static readonly Error TooManyRequests = Error.BadRequest("Too many requests");
    }
}