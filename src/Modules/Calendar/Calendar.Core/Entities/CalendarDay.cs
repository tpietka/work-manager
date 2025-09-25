namespace Calendar.Core.Entities;
internal abstract class CalendarDay
{
    protected CalendarDay(string name, DateOnly date)
    {
        Name = name;
        Date = date;
    }
    public DateOnly Date { get; set; }
    public List<DayEvent> Events { get; set; }
    public string Name { get; }
    public override string ToString()
    {
        return Name;
    }
}
