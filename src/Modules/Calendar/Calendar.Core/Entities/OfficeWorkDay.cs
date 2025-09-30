namespace Calendar.Core.Entities;

internal class OfficeWorkDay : WorkDay
{
    const string OFFICE_WORK_TYPE = "Praca stacjonarna";
    public OfficeWorkDay(DateOnly date) : base(OFFICE_WORK_TYPE, date)
    { }
}
