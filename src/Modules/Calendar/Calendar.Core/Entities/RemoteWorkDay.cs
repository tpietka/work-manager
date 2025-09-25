namespace Calendar.Core.Entities;

internal class RemoteWorkDay : CalendarDay
{
    const string REMOTE_WORK_TYPE = "Praca zdalna";
    public RemoteWorkDay(DateOnly date) : base(REMOTE_WORK_TYPE, date)
    {}
}
