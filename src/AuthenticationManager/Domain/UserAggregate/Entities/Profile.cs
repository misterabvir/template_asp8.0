using Domain.UserAggregate.Snapshots;
using Domain.UserAggregate.ValueObjects;

using Shared.Domain;

namespace Domain.UserAggregate.Entities;

public sealed class Profile : Entity<UserId>
{
    public FirstName FirstName { get; private set; } = null!;
    public LastName LastName { get; private set; } = null!;
    public ProfilePicture ProfilePicture { get; private set; } = null!;
    public CoverPicture CoverPicture { get; private set; } = null!;
    public Bio Bio { get; private set; } = null!;
    public Gender Gender { get; private set; } = null!;
    public Birthday Birthday { get; private set; } = null!;
    public Website Website { get; private set; } = null!;
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

    internal static Profile Create(UserId userId) => new(userId);

    internal ProfileSnapshot ToSnapshot() =>
        new()
        {
            FirstName = FirstName.Value,
            LastName = LastName.Value,
            ProfilePicture = LastName.Value,
            CoverPicture = LastName.Value,
            Bio = Bio.Value,
            Gender = Gender.Value,
            Birthday = Birthday.Value,
            Website = Website.Value,
            Location = Location.Value
        };

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