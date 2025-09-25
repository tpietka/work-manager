using Calendar.Core.Entities;
using WorkManager.Contracts.Services;
using WorkManager.TestHelpers;

namespace Calendar.Tests;

public class CalendarWeekTests
{
    private readonly static DateTime _dateTime = new(2025, 09, 25);
    
    private readonly IDateTimeProvider _dtProvider;

    public CalendarWeekTests()
    {
        _dtProvider = new FakeDateTimeProvider(_dateTime);
    }

    [Fact]
    public void AddDaysToWeek_ReturnOneDayInAWeek()
    {
        //Arrange
        var week = new CalendarWeek();
        var day = new RemoteWorkDay(_dtProvider.Today);

        //Act
        week.AddDay(day);

        //Assert
        Assert.True(week.Days.Count() == 1, "The week does not contain only one day");
    }

    [Fact]
    public void AddTwoDifferentWorkDaysToWeek_ReturnOneDayOfRemoteTypeInAWeek()
    {
        //Arrange
        var week = new CalendarWeek();
        var remoteDay = new RemoteWorkDay(_dtProvider.Today);
        var officeDay = new OfficeWorkDay(_dtProvider.Today.AddDays(1));

        //Act
        week.AddDay(remoteDay);
        week.AddDay(officeDay);

        //Assert
        Assert.True(week.Days.Count(x=>x.GetType() == typeof(RemoteWorkDay)) == 1, "The week does not contain only one remote day");
        Assert.True(week.Days.Count() == 2, "The week does not contain only one day");
    }

    [Fact]
    public void AddTwoDaysWithSameDate_ShouldThrowException()
    {
        //Arrange
        var week = new CalendarWeek();
        var remoteDay = new RemoteWorkDay(_dtProvider.Today);
        var officeDay = new OfficeWorkDay(_dtProvider.Today);

        //Act
        week.AddDay(remoteDay);

        //Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            week.AddDay(officeDay);
        });
        Assert.Equal("Day with same date already exists in a week", exception.Message);
    }
}
