using WorkManager.Contracts.Services;

namespace WorkManager.Infrastructure.Services;
public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => Now.ToLocalTime();
    public DateOnly Today => DateOnly.FromDateTime(Now);
}
