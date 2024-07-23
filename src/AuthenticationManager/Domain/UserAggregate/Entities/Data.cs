


using Domain.UserAggregate.ValueObjects;

using Shared.Domain;

namespace Domain.UserAggregate.Entities;

public sealed class Data : Entity<UserId>
{
    public Username Username{ get; private set; }
    public Email Email{ get; private set; }
    public Password Password{ get; private set; }
    public Salt Salt{ get; private set; }
    
    
    
    private Data(UserId userId, Username username, Email email, Password password, Salt salt){
        Id = userId;
        Username = username;
        Email = email;
        Password = password;
        Salt = salt;
    }

    internal static Data Create(UserId userId, Username username, Email email, Password password, Salt salt){
        return new Data(userId, username, email, password, salt);
    }

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