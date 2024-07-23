


using Domain.UserAggregate.Snapshots;
using Domain.UserAggregate.ValueObjects;

using Shared.Domain;

namespace Domain.UserAggregate.Entities;

public sealed class Data : Entity<UserId>
{
    public Username Username { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Password Password { get; private set; } = null!;
    public Salt Salt { get; private set; } = null!;

    private Data() { } //ef

    private Data(UserId userId, Username username, Email email, Password password, Salt salt)
    {
        Id = userId;
        Username = username;
        Email = email;
        Password = password;
        Salt = salt;
    }

    internal static Data Create(UserId userId, Username username, Email email, Password password, Salt salt)
    {
        return new Data(userId, username, email, password, salt);
    }

    internal DataSnapshot ToSnapshot() => new ()
    {
        Username = Username.Value,
        Email = Email.Value
    };

    internal void UpdatePassword(Password password, Salt salt)
    {
        Password = password;
        Salt = salt;
    }

    internal void UpdateEmail(Email email)
    {
        Email = email;
    }

    internal void UpdateUsername(Username username)
    {
        Username = username;
    }


}