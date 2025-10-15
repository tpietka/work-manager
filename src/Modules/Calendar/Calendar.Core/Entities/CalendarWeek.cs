using Microsoft.VisualBasic;

namespace Calendar.Core.Entities;
internal class CalendarWeek
{
    public IEnumerable<CalendarDay> Days => _days;

    private HashSet<CalendarDay> _days { get; set; } = [];

    public int DaysCount => Days.Count();

    public int RemoteWorkDaysCount => Days.Count(x => x is RemoteWorkDay);
    public int OfficeWorkDaysCount => Days.Count(x => x is OfficeWorkDay);
    public int WorkDaysCount => Days.Count(x => x is WorkDay);
    public int FreeDaysCount => Days.Count(x => x is FreeDay);

    public static CalendarWeek CreateRemoteWorkWeek(DateOnly startDate)
    {
        var startOfWeek = startDate.AddDays(-(int)startDate.DayOfWeek + 1);
        var week = new CalendarWeek();
        for (var i = 0; i < 5; i++)
        {
            week.AddDay(new RemoteWorkDay(startOfWeek.AddDays(i)));
        }
        AddFreeDays(startOfWeek, week);

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
        AddFreeDays(startOfWeek, week);
        return week;
    }

    private static void AddFreeDays(DateOnly startOfWeek, CalendarWeek week)
    {
        week.AddDay(new FreeDay(startOfWeek.AddDays(5)));
        week.AddDay(new FreeDay(startOfWeek.AddDays(6)));
    }

    public static CalendarWeek CreateCustomRemoteWorkWeek(params IEnumerable<DateOnly> days)
    {
        var firstDay = days.FirstOrDefault();
        var startOfWeek = firstDay.AddDays(-(int)firstDay.DayOfWeek + 1);
        var week = CreateOfficeWorkWeek(firstDay);

        foreach (var day in days)
        {
            week.ModifyDay(new RemoteWorkDay(day));
        }

        return week;
    }

    internal void AddDay(CalendarDay day)
    {
        if (_days.Select(x => x.Date).Contains(day.Date))
        {
            throw new InvalidOperationException("Day with same date already exists in a week");
        }

        ValidateSameWeek(day);

        _days.Add(day);
    }

    private void ValidateSameWeek(CalendarDay day)
    {
        if (_days.Count > 0 && !_days.First().IsSameWeek(day))
        {
            throw new InvalidOperationException("Can't add day from different week");
        }
    }

    internal void ModifyDay(CalendarDay day)
    {
        _days.RemoveWhere(d => d.Date == day.Date);

        ValidateSameWeek(day);

        _days.Add(day);
    }

    internal bool Includes(CalendarDay day)
    {
        return _days.First().IsSameWeek(day);
    }

    internal bool Includes(DateOnly day)
    {
        return _days.First().IsSameWeek(day);
    }

    internal bool IsDifferentWeek(CalendarWeek other)
    {
        return !_days.First().IsSameWeek(other.FirstDay());
    }

    internal CalendarDay FirstDay()
    {
        return _days.First();
    }

    internal DateOnly FirstWeekDate()
    {
        return _days.First().Date;
    }
}
