using Domain.UserAggregate.Entities;
using Domain.UserAggregate.Events;
using Domain.UserAggregate.ValueObjects;

using Shared.Domain;

namespace Domain.UserAggregate;

/// <summary>
/// Aggregate User
/// </summary>
public sealed class User : Aggregate<UserId>
{
    /// <summary> Role of the user in the system </summary>
    public Role Role { get; private set; } = null!;
    /// <summary> Status of the user in the system </summary>
    public Status Status { get; private set; } = null!;

    /// <summary> Data <see cref="Data"/> of the user in the system </summary>
    public Data Data { get; private set; } = null!;
    /// <summary> Profile <see cref="Profile"/> of the user in the system </summary>
    public Profile Profile { get; private set; } = null!;
    /// <summary>
    /// Date and time when the user was created
    /// </summary>
    public DateTime CreatedAt { get; private set; }
    /// <summary>
    /// Date and time when the user was last updated
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    private User() { } //ef
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

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="username">Username of the user</param>
    /// <param name="email">Email of the user</param>
    /// <param name="password">Password of the user</param>
    /// <param name="salt">Salt of the user</param>
    /// <returns></returns>
    public static User Create(Username username, Email email, Password password, Salt salt)
    {
        var userId = UserId.CreateUnique();
        var data = Data.Create(userId, username, email, password, salt);
        var profile = Profile.Create(userId);
        var user = new User(userId, data, profile);
        user.AddDomainEvent(new UserCreatedDomainEvent(userId.Value, username.Value, email.Value));
        return user;
    }

    /// <summary>
    /// Verify the user
    /// </summary>
    /// <returns></returns>
    public void Verify()
    {
        Status = Status.Active;
        AddDomainEvent(new UserVerifiedDomainEvent(Id.Value));
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Suspend the user
    /// </summary>
    /// <returns></returns>
    public void Suspend()
    {
        Status = Status.Suspended;
        AddDomainEvent(new UserSuspendedDomainEvent(Id.Value));
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Change the role of the user
    /// </summary>
    /// <param name="role">Role of the user</param>
    /// <returns></returns>
    public void ChangeRole(Role role)
    {
        Role = role;
        AddDomainEvent(new UserRoleChangedDomainEvent(Id.Value, Role.Value));
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Update the password of the user
    /// </summary>
    /// <param name="password">Password of the user</param>
    /// <param name="salt">Salt of the user</param>
    /// <returns></returns>
    public void UpdatePassword(Password password, Salt salt)
    {
        Data.UpdatePasswordAndSalt(password, salt);
        AddDomainEvent(new UserPasswordUpdatedDomainEvent(Id.Value));
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Update the data of the user
    /// </summary>
    /// <param name="email">Email of the user</param>
    /// <param name="username">Username of the user</param>
    /// <returns></returns>
    public void UpdateData(Email email, Username username)
    {
        Data.UpdateEmail(email);
        Data.UpdateUsername(username);
        AddDomainEvent(new UserDataUpdatedDomainEvent(Id.Value, Data.Email.Value, Data.Username.Value ));
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Update the profile of the user
    /// </summary>
    /// <param name="firstName">First name of the user</param>
    /// <param name="lastName">Last name of the user</param>
    /// <param name="profilePicture">Profile picture of the user</param>
    /// <param name="coverPicture">Cover picture of the user</param>
    /// <param name="bio">Bio of the user</param>
    /// <param name="gender">Gender of the user</param>
    /// <param name="birthday">Birthday of the user</param>
    /// <param name="website">Website of the user</param>
    /// <param name="location">Location of the user</param>
    /// <returns></returns>
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
        AddDomainEvent(new UserProfileUpdatedDomainEvent(Id.Value));
        UpdatedAt = DateTime.UtcNow;
    }
}


