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
        var day = new RemoteWorkDay(new DateOnly(2025, 12, 31));

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
        var remoteDay = new RemoteWorkDay(new DateOnly(2025, 09, 22));
        var officeDay = new OfficeWorkDay(new DateOnly(2025, 09, 22).AddDays(1));

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
        var remoteDay = new RemoteWorkDay(new DateOnly(2025, 09, 22));
        var officeDay = new OfficeWorkDay(new DateOnly(2025, 09, 22));

        //Act
        week.AddDay(remoteDay);

        //Assert
        Assert.Equal(1, week.RemoteWorkDaysCount);
        Assert.Equal(0, week.OfficeWorkDaysCount);

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            week.AddDay(officeDay);
        });

        Assert.Equal("Day with same date already exists in a week", exception.Message);
    }


    [Fact]
    public void ModifyTwoDaysWithSameDate_ShouldNotThrowException()
    {
        //Arrange
        var week = new CalendarWeek();
        var remoteDay = new RemoteWorkDay(new DateOnly(2025, 09, 22));
        var officeDay = new OfficeWorkDay(new DateOnly(2025, 09, 22));

        //Act
        week.AddDay(remoteDay);

        //Assert
        Assert.Equal(1, week.RemoteWorkDaysCount);
        Assert.Equal(0, week.OfficeWorkDaysCount);

        var exception = Record.Exception(() =>
        {
            week.ModifyDay(officeDay);
        });

        Assert.Null(exception);
        Assert.Equal(0, week.RemoteWorkDaysCount);
        Assert.Equal(1, week.OfficeWorkDaysCount);
    }


    [Fact]
    public void AddTwoDaysFromDifferentWeek_ShouldThrowException()
    {
        //Arrange
        var week = new CalendarWeek();
        var remoteDay = new RemoteWorkDay(new DateOnly(2025, 09, 22));
        var officeDay = new OfficeWorkDay(new DateOnly(2025, 09, 22).AddDays(8));

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

    [Fact]
    public void CreateWorkOnFreeDayOnWeekend_ShouldNotThrowException()
    {
        //Arrange & Act & Assert
        var exception = Record.Exception(() =>
        {
            var freeDay1 = new RemoteWorkOnFreeDay(new DateOnly(2025, 9, 27));
            Assert.Equal(27, freeDay1.Date.Day);
        });

        //Assert
        Assert.Null(exception);
    }

    [Fact]
    public void CreateRemoteWorkWeek_Has5WorkDaysAndTwoFreeDays()
    {
        //Arrange & Act
        var week = CalendarWeek.CreateRemoteWorkWeek(new DateOnly(2025, 09, 24));

        //Assert
        Assert.Multiple(
            () => Assert.Equal(5, week.WorkDaysCount),
            () => Assert.Equal(2, week.FreeDaysCount),
            () => Assert.Equal(new DateOnly(2025, 09, 22), week.Days.First().Date),
            () => Assert.Equal(new DateOnly(2025, 09, 28), week.Days.Last().Date),
            () => Assert.IsType<RemoteWorkDay>(week.Days.First()),
            () => Assert.IsType<FreeDay>(week.Days.Last()));
    }

    [Theory]
    [InlineData(2, "2025-09-24", "2025-09-25")]
    [InlineData(3, "2025-09-24", "2025-09-26", "2025-09-23")]
    public void CreateCustomRemoteWorkWeek_HasFullWeekWithGivenRemoteWorkDays(int expectedDaysCount, params string[] stringDates)
    {
        var days = stringDates.Select(DateOnly.Parse);

        var week = CalendarWeek.CreateCustomRemoteWorkWeek(days);

        Assert.Multiple(
            () => Assert.Equal(5, week.WorkDaysCount),
            () => Assert.Equal(expectedDaysCount, week.RemoteWorkDaysCount),
            () => Assert.Equal(2, week.FreeDaysCount),
            () => Assert.Equal(5-expectedDaysCount, week.OfficeWorkDaysCount),
            () => Assert.Equal(7, week.DaysCount),
            () => Assert.IsType<FreeDay>(week.Days.Last()));
    }

    [Theory]
    [InlineData("2025-09-24", "2025-09-30")]
    [InlineData("2025-09-24", "2025-09-26", "2025-09-19")]
    public void CreateCustomRemoteWorkWeekWithDifferentWeekDates_ShouldThrowException(params string[] stringDates)
    {
        var days = stringDates.Select(DateOnly.Parse);

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var week = CalendarWeek.CreateCustomRemoteWorkWeek(days);
        });
        Assert.Equal("Can't add day from different week", exception.Message);
    }
}
