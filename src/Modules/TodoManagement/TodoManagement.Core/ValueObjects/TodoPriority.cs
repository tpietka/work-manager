namespace TodoManagement.Core.ValueObjects;
public class TodoPriority : IEquatable<TodoPriority>
{
    public static readonly TodoPriority VeryLow = new(1);
    public static readonly TodoPriority Low = new(2);
    public static readonly TodoPriority Medium = new(3);
    public static readonly TodoPriority High = new(4);
    public static readonly TodoPriority VeryHigh = new(5);

    private static readonly Dictionary<int, string> PriorityNames = new()
    {
        { 1, "Very Low" },
        { 2, "Low" },
        { 3, "Medium" },
        { 4, "High" },
        { 5, "Very High" }
    };

    private TodoPriority() {
        Value = 3;
    }

    //public TodoPriority(int value)
    //{
    //    if (!PriorityNames.ContainsKey(value))
    //        throw new ArgumentException($"Priority must be between 1 and 5, got {value}", nameof(value));

    //    Value = value;
    //}

    public TodoPriority(int value)
    {
        if (value < 1 || value > 5)
            throw new ArgumentOutOfRangeException(nameof(value));
        Value = value;
    }

    public int Value { get; private set; }

    // Computed property - not stored in DB
    public string DisplayName => PriorityNames[Value];

    public bool Equals(TodoPriority? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => Equals(obj as TodoPriority);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => DisplayName;

    // Implicit conversions for convenience
    public static implicit operator int(TodoPriority priority) => priority.Value;
    public static implicit operator TodoPriority(int value) => new(value);
}
