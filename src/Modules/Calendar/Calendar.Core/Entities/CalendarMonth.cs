namespace Calendar.Core.Entities;
internal class CalendarMonth
{
    internal CalendarWeek FirstWeek { get; }
    internal CalendarWeek SecondWeek { get; }
    internal CalendarWeek ThirdWeek { get; }
    internal CalendarWeek FourthWeek { get; }

    private CalendarMonth(params CalendarWeek[] weeks)
    {
        weeks = weeks.OrderBy(x => x.FirstWeekDate()).ToArray();
        FirstWeek = weeks[0];
        SecondWeek = weeks[1];
        ThirdWeek = weeks[2];
        FourthWeek = weeks[3];
    }

    public static CalendarMonth CreateFromWeeks(CalendarWeek firstWeek, CalendarWeek secondWeek, CalendarWeek thirdWeek, CalendarWeek fourthWeek)
    {
        return new CalendarMonth(firstWeek, secondWeek, thirdWeek, fourthWeek);
    }

    public static CalendarMonth CreateFromDoubleWeeks(CalendarDoubleWeek firstDoubleWeek, CalendarDoubleWeek secondDoubleWeek)
    {
        return new CalendarMonth(firstDoubleWeek.FirstWeek, firstDoubleWeek.SecondWeek, secondDoubleWeek.FirstWeek, secondDoubleWeek.SecondWeek);
    }
}
