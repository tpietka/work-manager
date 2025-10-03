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
}
