using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Todo.Frontend.Models;

namespace Todo.Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<HomeController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var backendUrl = _configuration["BackendUrl"] ?? "http://localhost:5039";
            ViewBag.BackendUrl = backendUrl;

            List<TodoItem> todos = [];
            try
            {
                var client = _httpClientFactory.CreateClient("TodoBackend");
                var result = await client.GetFromJsonAsync<List<TodoItem>>("api/todos");
                if (result != null)
                {
                    todos = result;
                    ViewBag.IsBackendConnected = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch todos from backend at {BackendUrl}", backendUrl);
                ViewBag.IsBackendConnected = false;
                ViewBag.ErrorMessage = ex.Message;
            }

            return View(todos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
