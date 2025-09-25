namespace WorkManager.Contracts.Services;
public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
    DateOnly Today { get; }
}
