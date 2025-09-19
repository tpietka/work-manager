using TodoManagement.Core.Enums;
using TodoManagement.Core.ValueObjects;

namespace TodoManagement.Core.Entities;

public class TodoItem
{
    private TodoItem() { }

    public TodoItem(string title, DateTime createdAt, string? description = null, TodoPriority priority = null, DateTime? deadline = null)
    {
        Id = Guid.NewGuid();
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description;
        Status = TodoStatus.Todo;
        Priority = priority ?? TodoPriority.Medium;
        CreatedAt = createdAt;
        Deadline = deadline;
        AddLog("Created", null, "Todo item created", createdAt);
    }
    private readonly List<TodoLog> _logs = new();
    public IReadOnlyCollection<TodoLog> Logs => _logs.AsReadOnly();
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; } = string.Empty;
    public TodoStatus Status { get; private set; }
    public TodoPriority Priority { get; private set; } = TodoPriority.Medium;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? Deadline { get; private set; } = null;
    public DateTime? CompletedAt { get; private set; } = null;


    public void UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Title cannot be empty", nameof(newTitle));

        var oldValue = Title;
        Title = newTitle;
        UpdatedAt = DateTime.UtcNow;

        AddLog("TitleChanged", oldValue, newTitle, UpdatedAt);
    }

    public void UpdateDescription(string? newDescription)
    {
        var oldValue = Description;
        Description = newDescription;
        UpdatedAt = DateTime.UtcNow;

        AddLog("DescriptionChanged", oldValue, newDescription, UpdatedAt);
    }

    public bool CanChangePriority()
    {
        return Status != TodoStatus.Done;
    }

    public void ChangePriority(TodoPriority newPriority)
    {
        if (!CanChangePriority())
            throw new InvalidOperationException("Cannot change priority of completed todos");

        if (newPriority.Equals(Priority))
            return;

        var oldValue = Priority.Value;
        Priority = newPriority;
        UpdatedAt = DateTime.UtcNow;

        AddLog("PriorityChanged", oldValue.ToString(), newPriority.Value.ToString(), UpdatedAt);
    }

    public void ChangeStatus(TodoStatus newStatus)
    {
        var oldStatus = Status;
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;

        if (newStatus == TodoStatus.Done && oldStatus != TodoStatus.Done)
        {
            CompletedAt = DateTime.UtcNow;
        }
        else if (newStatus != TodoStatus.Done && oldStatus == TodoStatus.Done)
        {
            CompletedAt = null;
        }

        AddLog("StatusChanged", oldStatus.ToString(), newStatus.ToString(), UpdatedAt);
    }

    public void SetDeadline(DateTime? deadline)
    {
        var oldValue = Deadline?.ToString("yyyy-MM-dd HH:mm");
        Deadline = deadline;
        UpdatedAt = DateTime.UtcNow;

        AddLog("DeadlineChanged", oldValue, deadline?.ToString("yyyy-MM-dd HH:mm"), UpdatedAt);
    }

    private void AddLog(string action, string? oldValue, string? newValue, DateTime timestamp)
    {
        var log = new TodoLog(Id, action, oldValue, newValue, $"{action}: {oldValue} → {newValue}", timestamp);
        _logs.Add(log);
    }
}
