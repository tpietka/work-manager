namespace Calendar.Core.Entities;

internal class FreeDay : CalendarDay
{
    const string FREE_DAY = "Dzień wolny";
    public FreeDay(DateOnly date) : base(FREE_DAY, date)
    { }
}
