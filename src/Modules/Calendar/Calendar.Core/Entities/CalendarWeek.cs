namespace Calendar.Core.Entities;
internal class CalendarWeek
{
    public IEnumerable<CalendarDay> Days => _days;

    private HashSet<CalendarDay> _days { get; set; } = [];

    public int DaysCount => Days.Count();

    internal void AddDay(CalendarDay day)
    {
        if (_days.Select(x => x.Date).Contains(day.Date))
        {
            throw new InvalidOperationException("Day with same date already exists in a week");
        }

        if (_days.Count > 0 && !_days.First().IsSameWeek(day))
        {
            throw new InvalidOperationException("Can't add day from different week");
        }

        _days.Add(day);
    }
}
