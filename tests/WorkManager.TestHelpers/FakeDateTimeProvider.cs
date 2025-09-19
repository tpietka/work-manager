using WorkManager.Contracts.Services;

namespace WorkManager.TestHelpers;

public class FakeDateTimeProvider : IDateTimeProvider
{
    public DateTime Now { get; set; } = new DateTime(2025, 9, 9, 10, 0, 0);
    public DateTime UtcNow { get; set; } = new DateTime(2025, 9, 9, 10, 0, 0, DateTimeKind.Utc);
    public DateTimeOffset OffsetNow { get; set; } = new DateTimeOffset(2025, 9, 9, 10, 0, 0, TimeSpan.Zero);
    public DateTimeOffset OffsetUtcNow { get; set; } = new DateTimeOffset(2025, 9, 9, 10, 0, 0, TimeSpan.Zero);

    public void SetUtcNow(DateTime utcDateTime)
    {
        UtcNow = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
        OffsetUtcNow = new DateTimeOffset(utcDateTime, TimeSpan.Zero);
    }

    public void SetFixedTime(int year, int month, int day, int hour = 12, int minute = 0, int second = 0)
    {
        var fixedTime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
        SetUtcNow(fixedTime);
    }
}
