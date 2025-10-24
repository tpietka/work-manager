namespace Calendar.Core.Entities;
internal class CalendarYear
{
    public IEnumerable<CalendarWeek> Weeks { get; }    
    public CalendarYear(params CalendarWeek[] weeks)
    {
        Weeks = [.. weeks.DistinctBy(x => x.FirstWeekDate()).OrderBy(x => x.FirstWeekDate())];
    }
}
