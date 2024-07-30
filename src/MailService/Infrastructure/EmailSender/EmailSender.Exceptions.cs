namespace Infrastructure;



public static  partial class EmailSender
{
    public class EmailSettingsNotConfiguredException : Exception
    {
        public EmailSettingsNotConfiguredException() : base("Email settings not configured")
        {
        }
    }
}