namespace Infrastructure.BackgroundJobs;

public static partial class Outbox
{
    public class Settings{
        public const string SectionName = "Settings:Outbox";
        public required string JobKey { get; set; } 
        public required int IntervalInSeconds { get; set; }
        public required int MessagePerOneTime { get; set; }
    }
}