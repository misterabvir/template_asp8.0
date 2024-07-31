namespace AuthenticationService.Infrastructure.BackgroundJobs;

public static partial class Outbox
{ 
    public class OutboxSettingsNotConfiguredException : Exception
    {
        private const string DefaultMessage = "Outbox settings not configured";
        
        public OutboxSettingsNotConfiguredException() : base(DefaultMessage)
        {
        }
    }
}