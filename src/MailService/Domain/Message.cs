namespace Domain;

public class Message
{
    public Guid MessageId { get; set; }
    public Guid RecipientId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

