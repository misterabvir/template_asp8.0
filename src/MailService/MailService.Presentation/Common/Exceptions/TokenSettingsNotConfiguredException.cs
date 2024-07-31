namespace MailService.Presentation.Common.Exceptions;

public class TokenSettingsNotConfiguredException : Exception
{
    public TokenSettingsNotConfiguredException() : base("Token settings not configured")
    {
    }
}