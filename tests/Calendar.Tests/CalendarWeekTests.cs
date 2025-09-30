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
        Assert.True(week.Days.Count(x => x.GetType() == typeof(RemoteWorkDay)) == 1, "The week does not contain only one remote day");
        Assert.True(week.DaysCount == 2, "The week does not contain only one day");
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

    [Fact]
    public void AddTwoDaysFromDifferentWeek_ShouldThrowException()
    {
        //Arrange
        var week = new CalendarWeek();
        var remoteDay = new RemoteWorkDay(_dtProvider.Today);
        var officeDay = new OfficeWorkDay(_dtProvider.Today.AddDays(8));

        //Act
        week.AddDay(remoteDay);

        //Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            week.AddDay(officeDay);
        });
        Assert.Equal("Can't add day from different week", exception.Message);
    }

    [Fact]
    public void AddTwoDaysFromDifferetYears_ShouldNotThrowException()
    {
        //Arrange
        var week = new CalendarWeek();
        var remoteDay1 = new RemoteWorkDay(new DateOnly(2025, 12, 31));
        var remoteDay2 = new RemoteWorkDay(new DateOnly(2026, 1, 2));

        //Act
        week.AddDay(remoteDay1);

        //Assert
        week.AddDay(remoteDay2);
    }

    [Fact]
    public void AddTwoDaysFromSameWeekButDifferetYears_ShouldNotThrowException()
    {
        //Arrange
        var week = new CalendarWeek();
        var remoteDay1 = new RemoteWorkDay(new DateOnly(2025, 1, 2));
        var remoteDay2 = new RemoteWorkDay(new DateOnly(2026, 1, 2));

        //Act
        week.AddDay(remoteDay1);

        //Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            week.AddDay(remoteDay2);
        });
        Assert.Equal("Can't add day from different week", exception.Message);
    }

    [Fact]
    public void AddFreeDayOnWeekend_ShouldNotThrowException()
    {
        //Arrange
        var week = new CalendarWeek();
        var freeDay1 = new FreeDay(new DateOnly(2025, 9, 27));

        //Act
        week.AddDay(freeDay1);

        //Assert
        Assert.Equal(1, week.DaysCount);
    }

    [Fact]
    public void CreateWorkDayOnWeekend_ShouldThrowException()
    {
        //Arrange & Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var freeDay1 = new RemoteWorkDay(new DateOnly(2025, 9, 27));
        });
        Assert.Equal("Can't create workday on weekend", exception.Message);
    }
}
