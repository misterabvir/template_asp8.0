using Domain.UserAggregate.Entities;
using Domain.UserAggregate.Events;
using Domain.UserAggregate.ValueObjects;

using Shared.Domain;

namespace Domain.UserAggregate;

public sealed class User : Aggregate<UserId>
{
    public Role Role { get; private set; }
    public Status Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Data Data { get; private set; }
    public Profile Profile { get; private set; }
    private User(UserId userId, Data data, Profile profile)
    {
        Id = userId;
        Role = Role.User;
        Status = Status.NotVerified;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Data = data;
        Profile = profile;
    }

    public static User Create(Username username, Email email, Password password, Salt salt)
    {
        var userId = UserId.CreateUnique();
        var data = Data.Create(userId, username, email, password, salt);
        var profile = Profile.Create(userId);
        var user = new User(userId, data, profile);

        user.AddDomainEvent(new UserCreatedDomainEvent(user));

        return user;
    }

    public void Verify()
    {
        Status = Status.Active;
        AddDomainEvent(new UserVerifiedDomainEvent(this));
        UpdatedAt = DateTime.UtcNow;
    }

    public void Suspend()
    {
        Status = Status.Suspended;
        AddDomainEvent(new UserSuspendedDomainEvent(this));
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddRole(Role role)
    {
        Role = role;
        AddDomainEvent(new UserRoleAddedDomainEvent(this));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(Password password, Salt salt)
    {
        Data.UpdatePassword(password, salt);
        AddDomainEvent(new UserPasswordUpdatedDomainEvent(this));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateEmail(Email email)
    {
        Data.UpdateEmail(email);
        AddDomainEvent(new UserEmailUpdatedDomainEvent(this));
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdateUsername(Username username)
    {
        Data.UpdateUsername(username);
        AddDomainEvent(new UsernameUpdatedDomainEvent(this));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProfile(
        FirstName firstName,
        LastName lastName,
        ProfilePicture profilePicture,
        CoverPicture coverPicture,
        Bio bio,
        Gender gender,
        Birthday birthday,
        Website website,
        Location location)
    {
        Profile.Update(firstName, lastName, profilePicture, coverPicture, bio, gender, birthday, website, location);
        AddDomainEvent(new UserProfileUpdatedDomainEvent(this));
        UpdatedAt = DateTime.UtcNow;
    }
}


