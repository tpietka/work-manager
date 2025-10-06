using Calendar.Core.Entities;

namespace Calendar.Tests;

public class CalendarDoubleWeekTests
{
    [Fact]
    public void GetRemoteDays_ReturnOneDayInAWeek()
    {
        //Arrange
        var doubleWeek = CalendarDoubleWeek.CreateOfficeWorkWeekFirst(new DateOnly(2025, 10, 1));

        //Act
        var remoteDays = doubleWeek.GetRemoteDays();

        //Assert
        Assert.Equal(5, remoteDays.Count());
    }

    [Fact]
    public void CreateMixedWorkDoubleWeek_ShouldHaveRemoteDaysInBothWeeks()
    {
        var startDate = new DateOnly(2025, 09, 16);
        //Arrange
        var firstWeekDates = new List<DateOnly> {
            new DateOnly(2025, 09, 15),
            new DateOnly(2025, 09, 16),
            new DateOnly(2025, 09, 19),
            new DateOnly(2025, 09, 22),
            new DateOnly(2025, 09, 26),
        };

        //Act
        var doubleWeek = CalendarDoubleWeek.CreateMixedWorkWeekFirst(startDate, firstWeekDates);

        //Assert
        Assert.Equal(5, doubleWeek.GetRemoteDays().Count());
        Assert.Equal(3, doubleWeek.FirstWeek.RemoteWorkDaysCount);
        Assert.Equal(2, doubleWeek.SecondWeek.RemoteWorkDaysCount);
    }

    [Fact]
    public void CreateMixedWorkDoubleWeekWithMoreThanFiveRemoteDays_ShouldThrowException()
    {
        //Arrange
        var startDate = new DateOnly(2025, 09, 16);
        var dates = new List<DateOnly> {
            new DateOnly(2025, 09, 15),
            new DateOnly(2025, 09, 16),
            new DateOnly(2025, 09, 19),
            new DateOnly(2025, 09, 22),
            new DateOnly(2025, 09, 23),
            new DateOnly(2025, 09, 26),
        };

        //Act
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var doubleWeek = CalendarDoubleWeek.CreateMixedWorkWeekFirst(startDate, dates);
        });

        //Assert
        Assert.Equal("Can't create double week with more or less than 5 remote days", exception.Message);
    }

    [Fact]
    public void CreateFirstWeekRemoteOnly_ShouldHaveSecondWeekInOffice()
    {
        //Arrange
        var startDate = new DateOnly(2025, 09, 16);
        var dates = new List<DateOnly> {
            new DateOnly(2025, 09, 15),
            new DateOnly(2025, 09, 16),
            new DateOnly(2025, 09, 17),
            new DateOnly(2025, 09, 18),
            new DateOnly(2025, 09, 19),
        };

        //Act
        var doubleWeek = CalendarDoubleWeek.CreateMixedWorkWeekFirst(startDate, dates);

        //Assert
        Assert.Equal(5, doubleWeek.FirstWeek.RemoteWorkDaysCount);
        Assert.Equal(5, doubleWeek.SecondWeek.OfficeWorkDaysCount);
    }

    [Fact]
    public void CreateSecondWeekRemoteOnly_ShouldHaveFirstWeekInOffice()
    {
        //Arrange
        var startDate = new DateOnly(2025, 09, 09);
        var dates = new List<DateOnly> {
            new DateOnly(2025, 09, 15),
            new DateOnly(2025, 09, 16),
            new DateOnly(2025, 09, 17),
            new DateOnly(2025, 09, 18),
            new DateOnly(2025, 09, 19),
        };

        //Act
        var doubleWeek = CalendarDoubleWeek.CreateMixedWorkWeekFirst(startDate, dates);

        //Assert
        Assert.Equal(5, doubleWeek.FirstWeek.OfficeWorkDaysCount);
        Assert.Equal(5, doubleWeek.SecondWeek.RemoteWorkDaysCount);
    }

    [Fact]
    public void CreateMixedWorkDoubleWeekWithLessThanFiveRemoteDays_ShouldThrowException()
    {
        //Arrange
        var startDate = new DateOnly(2025, 09, 16);
        var firstWeekDates = new List<DateOnly> {
            new DateOnly(2025, 09, 15),
            new DateOnly(2025, 09, 16),
            new DateOnly(2025, 09, 23),
            new DateOnly(2025, 09, 26),
        };

        //Act
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var doubleWeek = CalendarDoubleWeek.CreateMixedWorkWeekFirst(startDate, firstWeekDates);
        });

        //Assert
        Assert.Equal("Can't create double week with more or less than 5 remote days", exception.Message);
    }
}
