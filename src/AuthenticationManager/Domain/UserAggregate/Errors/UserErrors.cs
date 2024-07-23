using Shared.Results;

namespace Domain.UserAggregate;

public static class Errors
{

    public static class Users
    {
        public readonly static Error FailDeserializeSnapshot = Error.InternalServerError("Failed to deserialize snapshot");
    }
}

