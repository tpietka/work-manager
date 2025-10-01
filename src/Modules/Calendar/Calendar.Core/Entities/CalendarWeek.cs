namespace Calendar.Core.Entities;
internal class CalendarWeek
{
    public IEnumerable<CalendarDay> Days => _days;

    private HashSet<CalendarDay> _days { get; set; } = [];

    public int DaysCount => Days.Count();

    public int WorkDaysCount => Days.Count(x=> x is WorkDay);
    public int FreeDaysCount => Days.Count(x => x is FreeDay);

    public static CalendarWeek CreateRemoteWorkWeek(DateOnly startDate)
    {
        var startOfWeek = startDate.AddDays(-(int)startDate.DayOfWeek + 1);
        var week = new CalendarWeek();
        for (var i = 0; i < 5; i++) {
            week.AddDay(new RemoteWorkDay(startOfWeek.AddDays(i)));
        }
        week.AddDay(new FreeDay(startOfWeek.AddDays(5)));
        week.AddDay(new FreeDay(startOfWeek.AddDays(6)));
        return week;
    }

    public static CalendarWeek CreateOfficeWorkWeek(DateOnly startDate)
    {
        var startOfWeek = startDate.AddDays(-(int)startDate.DayOfWeek + 1);
        var week = new CalendarWeek();
        for (var i = 0; i < 5; i++)
        {
            week.AddDay(new OfficeWorkDay(startOfWeek.AddDays(i)));
        }
        week.AddDay(new FreeDay(startOfWeek.AddDays(5)));
        week.AddDay(new FreeDay(startOfWeek.AddDays(6)));
        return week;
    }

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
