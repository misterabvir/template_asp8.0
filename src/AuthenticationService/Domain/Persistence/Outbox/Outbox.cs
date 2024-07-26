using System.ComponentModel.DataAnnotations;

namespace Domain.Persistence;

/// <summary>
/// Outbox pattern message
/// </summary>
public static partial class Outbox
{
    public class Message
    {
        [Key]public Guid OutboxMessageId { get; set; } = Guid.NewGuid();
        public string Type { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }
        public bool IsProcessed => ProcessedAt is not null;
    }
}
