namespace Calendar.Core.Entities;

internal abstract class WorkDay : CalendarDay
{
    protected WorkDay(string dayTypeName, DateOnly date) : base(dayTypeName, date)
    { 
        if(InvalidDayType())
        {
            throw new InvalidOperationException("Can't create workday on weekend");
        }
    }

    internal bool InvalidDayType()
    {
        return IsWeekend;
    }
}
