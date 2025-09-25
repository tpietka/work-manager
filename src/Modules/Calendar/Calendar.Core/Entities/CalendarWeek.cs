using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Core.Entities;
internal class CalendarWeek
{
    public IEnumerable<CalendarDay> Days => _days;
    private HashSet<CalendarDay> _days { get; set; } = [];

    internal void AddDay(CalendarDay day)
    {
        if(_days.Select(x=>x.Date).Contains(day.Date))
        {
            throw new InvalidOperationException("Day with same date already exists in a week");
        }
        _days.Add(day);
    }
}
