﻿namespace MailService.Domain;

public class Message
{
    public Guid MessageId { get; set; } = Guid.NewGuid();
    public Guid RecipientId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? Recipient{ get; set; }  
}

