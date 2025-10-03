namespace Calendar.Core.Entities;

internal class CalendarDoubleWeek
{
    private readonly CalendarWeek _firstWeek;
    private readonly CalendarWeek _secondWeek;

    public CalendarDoubleWeek(CalendarWeek firstWeek, CalendarWeek secondWeek)
    {
        (_firstWeek, _secondWeek) = (firstWeek, secondWeek);
    }

    public static CalendarDoubleWeek CreateRemoteWorkWeekFirst(DateOnly startDate)
    {
        var firstWeek = CalendarWeek.CreateRemoteWorkWeek(startDate);
        var secondWeek = CalendarWeek.CreateOfficeWorkWeek(startDate.AddDays(7));

        return new CalendarDoubleWeek(firstWeek, secondWeek);
    }

    public static CalendarDoubleWeek CreateOfficeWorkWeekFirst(DateOnly startDate)
    {
        var firstWeek = CalendarWeek.CreateOfficeWorkWeek(startDate);
        var secondWeek = CalendarWeek.CreateRemoteWorkWeek(startDate.AddDays(7));

        return new CalendarDoubleWeek(firstWeek, secondWeek);
    }

    public IEnumerable<CalendarDay> GetRemoteDays()
    {
        var allDays = new List<CalendarDay>();
        allDays.AddRange(_firstWeek.Days);
        allDays.AddRange(_secondWeek.Days);

        return allDays.Where(d => d is RemoteWorkDay);
    }
}
