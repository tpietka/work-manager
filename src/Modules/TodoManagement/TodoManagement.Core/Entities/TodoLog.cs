namespace TodoManagement.Core.Entities;
public class TodoLog
{
    private TodoLog() { }

    public TodoLog(Guid todoItemId, string action, string? oldValue, string? newValue, string description, DateTime timestamp)
    {
        Id = Guid.NewGuid();
        TodoItemId = todoItemId;
        Action = action ?? throw new ArgumentNullException(nameof(action));
        OldValue = oldValue;
        NewValue = newValue;
        Description = description ?? throw new ArgumentNullException(nameof(description));
        ChangedAt = timestamp;
    }

    public Guid Id { get; private set; }
    public Guid TodoItemId { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public string? OldValue { get; private set; }
    public string? NewValue { get; private set; }
    public DateTime ChangedAt { get; private set; }
    public string Description { get; private set; } = string.Empty;

    // Navigation property
    public TodoItem TodoItem { get; private set; } = null!;
}
