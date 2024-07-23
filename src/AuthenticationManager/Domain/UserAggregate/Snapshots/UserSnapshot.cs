namespace Domain.UserAggregate.Snapshots;

public class UserSnapshot
{
    public Guid UserId { get; init; }
    public string Role { get; init; } = null!;
    public string Status { get; init; } = null!;
    public DataSnapshot Data { get; init; } = null!;
    public ProfileSnapshot Profile { get; init; } = null!;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
