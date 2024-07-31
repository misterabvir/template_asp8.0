using AuthenticationService.Domain.UserAggregate.ValueObjects;

using Domain.Abstractions;

namespace AuthenticationService.Domain.UserAggregate.Entities;

/// <summary>
/// Represents a user's profile.
/// </summary>
/// <remarks>
/// This class is marked as sealed to prevent inheritance and ensure that the Profile object is created through the static Create method.
/// </remarks>
public sealed class Profile : Entity<UserId>
{
    /// <summary> First name </summary>
    public FirstName FirstName { get; private set; } = null!;
    /// <summary> Last name </summary>
    public LastName LastName { get; private set; } = null!;
    /// <summary> Profile picture </summary>
    /// <remarks>
    /// This property represents the user's profile picture.
    /// </remarks>
    /// <example>
    /// Example value: "https://example.com/profile.jpg"
    /// </example>
    public ProfilePicture ProfilePicture { get; private set; } = null!;
    /// <summary> Cover picture </summary>
    /// <remarks>
    /// This property represents the user's cover picture.
    /// </remarks>
    /// <example>
    /// Example value: "https://example.com/cover.jpg"
    /// </example>
    public CoverPicture CoverPicture { get; private set; } = null!;
    /// <summary> Bio </summary>
    /// <remarks>
    /// This property represents the user's bio.
    /// </remarks>
    /// <example>
    /// Example value: "Hello, my name is John Doe and I am a software engineer."
    /// </example>
    public Bio Bio { get; private set; } = null!;
    /// <summary> Gender </summary>
    /// <remarks>
    /// This property represents the user's gender.
    /// </remarks>
    /// <example>
    /// Example value: "Male"
    /// </example>
    public Gender Gender { get; private set; } = null!;
    /// <summary> Birthday </summary>
    /// <remarks>
    /// This property represents the user's birthday.
    /// </remarks>
    /// <example>
    /// Example value: "1990-01-01"
    /// </example>
    public Birthday Birthday { get; private set; } = null!;
    /// <summary> Website </summary>
    /// <remarks>
    /// This property represents the user's website.
    /// </remarks>
    /// <example>
    /// Example value: "https://example.com"
    /// </example>
    public Website Website { get; private set; } = null!;
    /// <summary> Location </summary>
    /// <remarks>
    /// This property represents the user's location.
    /// </remarks>
    /// <example>
    /// Example value: "New York, NY"
    /// </example>
    public Location Location { get; private set; } = null!;

    private Profile() { } //ef
    private Profile(UserId userId)
    {
        Id = userId;
        FirstName = FirstName.Empty;
        LastName = LastName.Empty;
        ProfilePicture = ProfilePicture.Empty;
        CoverPicture = CoverPicture.Empty;
        Gender = Gender.Empty;
        Birthday = Birthday.Empty;
        Website = Website.Empty;
        Location = Location.Empty;
        Bio = Bio.Empty;
    }

    /// <summary>
    /// Creates a new profile for the given user.
    /// </summary>
    /// <remarks>
    /// This method is used to create a new profile for a user.
    /// </remarks>
    /// <param name="userId"></param>
    /// <returns></returns>
    internal static Profile Create(UserId userId) => new(userId);

    /// <summary>
    /// Updates the profile with the given values.
    /// </summary>
    /// <remarks>
    /// This method is used to update the profile with the given values.
    /// </remarks>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="profilePicture"></param>
    /// <param name="coverPicture"></param>
    /// <param name="bio"></param>
    /// <param name="gender"></param>   
    /// <param name="birthday"></param>
    /// <param name="website"></param>
    /// <param name="location"></param>
    /// <returns></returns>
    internal void Update(FirstName firstName, LastName lastName, ProfilePicture profilePicture, CoverPicture coverPicture, Bio bio, Gender gender, Birthday birthday, Website website, Location location)
    {
        FirstName = firstName;
        LastName = lastName;
        ProfilePicture = profilePicture;
        CoverPicture = coverPicture;
        Bio = bio;
        Gender = gender;
        Birthday = birthday;
        Website = website;
        Location = location;

    }
}