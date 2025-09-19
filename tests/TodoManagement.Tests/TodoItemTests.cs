using FluentAssertions;
using TodoManagement.Core.Entities;
using TodoManagement.Core.Enums;
using TodoManagement.Core.ValueObjects;

namespace TodoManagement.Tests;
public class TodoItemTests
{
    [Fact]
    public void Constructor_ValidParameters_ShouldCreateTodoItem()
    {
        //Arrange
        var title = "Todo title";
        var description = "Todo description";
        var priority = TodoPriority.High;
        var date = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var deadline = date.AddDays(7);
        //Act
        var todo = new TodoItem(title, date, description, priority, deadline);

        //Assert
        todo.Id.Should().NotBe(Guid.Empty);
        todo.Title.Should().Be(title);
        todo.Description.Should().Be(description);
        todo.Priority.Should().Be(priority);
        todo.Status.Should().Be(TodoStatus.Todo);
        todo.Deadline.Should().Be(deadline);
        todo.CreatedAt.Should().BeCloseTo(date, TimeSpan.FromSeconds(1));
        todo.Logs.Should().HaveCount(1);
        todo.Logs.First().Action.Should().Be("Created");
    }

    [Fact]
    public void Constructor_NullTitle_ShouldThrowArgumentNullException()
    {
        var date = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc);

        var action = () => new TodoItem(null, date);
        action.Should().Throw<ArgumentNullException>().WithParameterName("title");
    }
}
