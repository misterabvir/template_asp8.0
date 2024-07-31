namespace AuthenticationService.Infrastructure;
public static partial class Verifications
{
    public class VerifySettingsNotConfiguredException : Exception
    {
        public const string DefaultMessage = "Verify settings not configured.";
        public VerifySettingsNotConfiguredException() : base(DefaultMessage)
        {
        }
    }

}