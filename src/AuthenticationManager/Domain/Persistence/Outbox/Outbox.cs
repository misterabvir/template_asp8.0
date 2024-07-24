namespace Domain.Persistence;

public static partial class Outbox
{
    public class Message
    {
        public Guid OutboxMessageId { get; set; } = Guid.NewGuid();
        public string Type { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }
        public bool IsProcessed => ProcessedAt is not null;
    }
}
