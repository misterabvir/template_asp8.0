using Domain.UserAggregate.ValueObjects;
using Shared.Domain;

namespace Domain.UserAggregate.Entities;

/// <summary>
/// User's data
/// </summary>
public sealed class Data : Entity<UserId>
{
    /// <summary> Username </summary>
    /// <remarks>
    /// This property represents the user's username.
    /// </remarks>
    public Username Username { get; private set; } = null!;
    /// <summary> Email </summary>
    /// <remarks>
    /// This property represents the user's email address.
    /// </remarks>
    public Email Email { get; private set; } = null!;
    /// <summary> Password </summary>
    /// <remarks>
    /// This property represents the user's password.
    /// </remarks>
    public Password Password { get; private set; } = null!;
    /// <summary> Salt </summary>
    /// <remarks>
    /// This property represents the user's salt used for password hashing.
    /// </remarks>
    /// <value></value>
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

    /// <summary>
    /// Create a new instance of Data
    /// </summary>
    /// <param name="userId">User's id</param>
    /// <param name="username">User's username</param>
    /// <param name="email">User's email</param>
    /// <param name="password">User's password</param>
    /// <param name="salt">User's salt</param>
    /// <returns>A new instance of Data</returns>
    internal static Data Create(UserId userId, Username username, Email email, Password password, Salt salt)
    {
        return new Data(userId, username, email, password, salt);
    }   

    /// <summary>
    /// Update the password and salt of the user
    /// </summary>
    /// <param name="password"></param>
    /// <param name="salt"></param>
    internal void UpdatePasswordAndSalt(Password password, Salt salt)
    {
        Password = password;
        Salt = salt;
    }

    /// <summary>
    /// Update the username of the user
    /// </summary>
    /// <param name="username"></param>
    internal void UpdateUsername(Username username)
    {
        Username = username;
    }


}