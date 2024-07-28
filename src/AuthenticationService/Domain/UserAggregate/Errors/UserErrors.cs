using Shared.Results;

namespace Domain.UserAggregate;

public static class Errors
{

    public static class Users
    {
        /// <summary> User not found in database </summary>
        public readonly static Error NotFound = Error.NotFound("User not found");
        
        /// <summary> Invalid Credentials  </summary>
        public readonly static Error InvalidCredentials = Error.BadRequest("Invalid Credentials");
       
        /// <summary> Username already taken </summary>
        public readonly static Error UsernameAlreadyTaken = Error.Conflict("Username already taken");
        
        /// <summary> Email already taken </summary>
        public readonly static Error EmailAlreadyTaken = Error.Conflict("Email already taken");
        
        /// <summary> User is already active </summary>
        public readonly static Error AlreadyActive = Error.Conflict("User is already active");
        
        /// <summary> User is not required permissions  </summary>
        public readonly static Error NotHavePermission = Error.Forbidden("User does not have permission");
        
        /// <summary> User already have this role </summary>
        public  readonly static Error AlreadyHaveThisRole = Error.Conflict("User already have this role");
    }
}

