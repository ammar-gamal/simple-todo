using Microsoft.AspNetCore.Mvc;
using Todo.Backend.Controllers;
using Todo.Backend.Models;
using Xunit;

namespace Todo.Backend.Tests;

public class TodosControllerTests
{
    [Fact]
    public void GetAll_ReturnsOkResult_WithListOfTodos()
    {
        // Arrange
        var controller = new TodosController();

        // Act
        var result = controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var todos = Assert.IsAssignableFrom<IEnumerable<TodoItem>>(okResult.Value);
        Assert.NotEmpty(todos);
    }

    [Fact]
    public void GetAll_ContainsExpectedItems()
    {
        // Arrange
        var controller = new TodosController();

        // Act
        var result = controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var todos = Assert.IsAssignableFrom<IEnumerable<TodoItem>>(okResult.Value);
        Assert.Contains(todos, item => item.Id == 1 && item.Title.Contains("Provision Cloud Resources"));
    }
}
