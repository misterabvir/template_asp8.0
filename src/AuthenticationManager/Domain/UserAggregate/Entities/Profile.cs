using Domain.UserAggregate.ValueObjects;

using Shared.Domain;

namespace Domain.UserAggregate.Entities;

public sealed class Profile : Entity<UserId>
{
    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public ProfilePicture ProfilePicture { get; private set; }
    public CoverPicture CoverPicture { get; private set; }
    public Bio Bio { get; private set; }
    public Gender Gender { get; private set; }
    public Birthday Birthday { get; private set; }
    public Website Website { get; private set; }
    public Location Location { get; private set; }
    
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

    internal static Profile Create(UserId userId)
    {
        return new Profile(userId);
    }
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