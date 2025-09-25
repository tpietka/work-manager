using WorkManager.Contracts.Services;

namespace WorkManager.TestHelpers;

public class FakeDateTimeProvider : IDateTimeProvider
{
    public DateTime Now { get; }
    public DateTime UtcNow => Now.ToLocalTime();
    public DateOnly Today => DateOnly.FromDateTime(Now);

    public FakeDateTimeProvider(DateTime dateTime)
    {
        Now = dateTime;
    }

    public FakeDateTimeProvider() : this(new DateTime(2025, 9, 9, 10, 0, 0)) 
    { 
    }
}
