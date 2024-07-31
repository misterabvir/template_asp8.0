namespace MailService.Presentation.Common.Exceptions;

public class QueueConnectionNotConfiguredException : Exception
{
    public  QueueConnectionNotConfiguredException() : base("Queue connection not configured")
    {
    }
}