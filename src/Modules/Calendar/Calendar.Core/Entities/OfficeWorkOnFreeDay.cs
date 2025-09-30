namespace Calendar.Core.Entities;

internal class OfficeWorkOnFreeDay : WorkDay
{
    const string OFFICE_WORK_ON_FREE_DAY_TYPE = "Praca stacjonarna w dzień wolny";
    public OfficeWorkOnFreeDay(DateOnly date) : base(OFFICE_WORK_ON_FREE_DAY_TYPE, date)
    { }
}
