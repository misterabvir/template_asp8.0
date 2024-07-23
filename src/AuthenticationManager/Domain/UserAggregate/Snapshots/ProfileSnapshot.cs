namespace Domain.UserAggregate.Snapshots;

public class ProfileSnapshot
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string ProfilePicture { get; init; } = null!;
    public string CoverPicture { get; init; } = null!;
    public string Bio { get; init; } = null!;
    public string Gender { get; init; } = null!;
    public DateOnly Birthday { get; init; }
    public string Website { get; init; } = null!;
    public string Location { get; init; } = null!;
}