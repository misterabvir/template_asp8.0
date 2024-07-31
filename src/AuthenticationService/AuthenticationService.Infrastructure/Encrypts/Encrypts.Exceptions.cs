namespace AuthenticationService.Infrastructure;
public static partial class Encrypts
{

    public class EncryptsSettingsNotConfiguredException : Exception
    {
        private const string DefaultMessage = "Encrypts settings not configured";
        public EncryptsSettingsNotConfiguredException() : base(DefaultMessage)
        {
        }
    }
}