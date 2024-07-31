using Shared.Domain;

namespace AuthenticationService.Domain.UserAggregate.ValueObjects;

public record Role : ValueObject
{
    public string Value { get; init; }
    private Role(string value) => Value = value;
    public static Role Create(string value) => new (value);
    public static Role Administrator => new (AuthorizationConstants.Roles.Administrator);
    public static Role User => new (AuthorizationConstants.Roles.User);


}
