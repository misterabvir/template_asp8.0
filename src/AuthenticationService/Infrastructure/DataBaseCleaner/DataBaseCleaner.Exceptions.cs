namespace Infrastructure;

public static partial class DataBaseCleaner
{
    private const string DefaultMessage = "DataBaseCleaner settings not configured";
    public class DatabaseCleanerSettingsNotConfiguredException : Exception
    {
        public DatabaseCleanerSettingsNotConfiguredException() : base(DefaultMessage)
        {
        }
    }
}