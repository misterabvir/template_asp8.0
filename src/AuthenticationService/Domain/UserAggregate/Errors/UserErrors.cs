using Shared.Results;

namespace Domain.UserAggregate;

public static class Errors
{

    public static class Users
    {
        public readonly static Error FailDeserializeSnapshot = Error.InternalServerError("Failed to deserialize snapshot");
        public readonly static Error NotFound = Error.NotFound("User not found");
        public readonly static Error InvalidCredentials = Error.NotFound("Invalid Credentials");
        public readonly static Error UsernameAlreadyTaken = Error.Conflict("Username already taken");
        public readonly static Error EmailAlreadyTaken = Error.Conflict("Email already taken");

        public readonly static Error AlreadyActive = Error.Conflict("User is already active");

        public readonly static Error NotHavePermission = Error.Forbidden("User does not have permission");

        public  readonly static Error AlreadyHaveThisRole = Error.Conflict("User already have this role");
    }
}

