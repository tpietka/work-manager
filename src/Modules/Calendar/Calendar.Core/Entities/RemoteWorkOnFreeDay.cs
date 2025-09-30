namespace Calendar.Core.Entities;

internal class RemoteWorkOnFreeDay : WorkDay
{
    const string REMOTE_WORK_ON_FREE_DAY_TYPE = "Praca zdalna w dzień wolny";
    public RemoteWorkOnFreeDay(DateOnly date) : base(REMOTE_WORK_ON_FREE_DAY_TYPE, date)
    { }
}
