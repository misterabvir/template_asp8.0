namespace AuthenticationService.Infrastructure.BackgroundJobs;

public static partial class DataBaseCleaner
{

    public class Settings
    {
        public const string SectionName = "Settings:DataBaseCleaner";
        public required string JobKey { get; set; }
        public required int NotVerifiedTimeLimitInHours { get; set; } = 24;
        public required int OldMessagesTimeLimitInDays { get; set; } = 7;
        public required int IntervalInSeconds { get; set; } = 3600;
    }
}