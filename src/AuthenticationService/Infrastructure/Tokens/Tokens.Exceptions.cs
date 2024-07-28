namespace Infrastructure;

public static partial class Tokens
{
    public class TokenSettingsNotConfiguredException : Exception
    {
        public TokenSettingsNotConfiguredException() : base("Token settings not configured.") { }
    }
}