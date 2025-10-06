namespace Calendar.Core.Entities;

internal class CalendarDoubleWeek
{
    private readonly CalendarWeek _firstWeek;
    private readonly CalendarWeek _secondWeek;
    private const int MAX_REMOTE_DAYS = 5;

    public CalendarWeek FirstWeek => _firstWeek;
    public CalendarWeek SecondWeek => _secondWeek;

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

    public static CalendarDoubleWeek CreateMixedWorkWeekFirst(DateOnly startDate, List<DateOnly> remoteWorkDates)
    {
        if (remoteWorkDates.Count() != MAX_REMOTE_DAYS)
        {
            throw new InvalidOperationException("Can't create double week with more or less than 5 remote days");
        }

        var firstWeek = CalendarWeek.CreateOfficeWorkWeek(startDate);
        var secondWeek = CalendarWeek.CreateOfficeWorkWeek(firstWeek.FirstWeekDate().AddDays(7));

        remoteWorkDates = remoteWorkDates.OrderBy(x => x).ToList();

        foreach(var day in remoteWorkDates)
        {
            if (firstWeek.Includes(day))
            {
                firstWeek.ModifyDay(new RemoteWorkDay(day));
            }
            else if (secondWeek.Includes(day))
            {
                secondWeek.ModifyDay(new RemoteWorkDay(day));
            }
            else
            {
                throw new InvalidOperationException("Day does not belong to first or second week");
            }
        }

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
