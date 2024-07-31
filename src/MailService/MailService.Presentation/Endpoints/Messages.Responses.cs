namespace MailService.Presentation.Endpoints;

public static partial class Messages
{
    public static class Responses
    {
        public class Message
        {
            public string Username { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public string Reason { get; set; } = string.Empty;
        }
    }


    public static Responses.Message ToResponse(this MailService.Domain.Message message)
    {
        return new Responses.Message
        {
            Username = message.User?.Username ?? "",
            Email = message.User?.Email ?? "",
            Type = message.Type,
            Reason = message.Reason
        };
    }
    public static IEnumerable<Responses.Message> ToResponse(this IEnumerable<MailService.Domain.Message> messages)
    {
        return messages.Select(m => m.ToResponse());
    }   
}