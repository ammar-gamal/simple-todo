using Microsoft.AspNetCore.Mvc;
using Todo.Backend.Models;

namespace Todo.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private static readonly List<TodoItem> Todos = new()
    {
        new TodoItem { Id = 1, Title = "Provision Cloud Resources (Container Apps / AKS)", IsCompleted = true },
        new TodoItem { Id = 2, Title = "Deploy Todo Backend API to Cloud", IsCompleted = true },
        new TodoItem { Id = 3, Title = "Deploy Todo Frontend and Connect via Internal Network", IsCompleted = true },
        new TodoItem { Id = 4, Title = "Verify Cloud Scaling & Health Probes", IsCompleted = false },
        new TodoItem { Id = 5, Title = "Configure Cloud Environment Variables & Secrets", IsCompleted = false }
    };

    [HttpGet]
    public ActionResult<IEnumerable<TodoItem>> GetAll()
    {
        return Ok(Todos);
    }
}
