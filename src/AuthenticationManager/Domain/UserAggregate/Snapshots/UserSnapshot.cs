using System.Text.Json;
using Shared.Results;

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

    public string ToJson() => JsonSerializer.Serialize(this);
    public static Result<UserSnapshot> FromJson(string json)
    {
        var snapshot = JsonSerializer.Deserialize<UserSnapshot>(json);
        return snapshot is null ? Errors.Users.FailDeserializeSnapshot : snapshot;
    }
}
