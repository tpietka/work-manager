using System.Globalization;

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
    public bool IsWeekend => Date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    public bool IsSameWeek(CalendarDay other)
    {
        DateTime date1 = Date.ToDateTime(TimeOnly.MinValue);
        DateTime date2 = other.Date.ToDateTime(TimeOnly.MinValue);

        var week1 = ISOWeek.GetWeekOfYear(date1);
        var week2 = ISOWeek.GetWeekOfYear(date2);
        var year1 = ISOWeek.GetYear(date1);
        var year2 = ISOWeek.GetYear(date2);

        return week1 == week2 && year1 == year2;
    }
}
