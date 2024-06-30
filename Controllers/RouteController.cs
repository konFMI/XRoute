using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using XRoute.Data;
using XRoute.Models;

[Authorize]
public class RouteController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<RouteController> _logger;

    public RouteController(ApplicationDbContext context, ILogger<RouteController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Authorize(Roles = "Representative, Normal")]
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Roles = "Representative, Normal")]
    [HttpGet]
    public IActionResult List(string category)
    {
        if (string.IsNullOrEmpty(category))
        {
            _logger.LogWarning("Category is null or empty.");
            return BadRequest("Category cannot be null or empty.");
        }

        var routes = _context.Routes.Where(r => r.Category == category).OrderByDescending(r => r.DateAdded).ToList();
        ViewData["Title"] = category + " Routes";
        return View(routes);
    }

    [Authorize(Roles = "Representative")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Representative")]
    [HttpPost]
    public async Task<IActionResult> Create(XRoute.Models.Route model)
    {
        if (User.Identity?.Name == null)
        {
            _logger.LogWarning("User identity is null.");
            return RedirectToAction("Login", "Account");
        }

        if (ModelState.IsValid)
        {
            model.RepresentativeUsername = User.Identity.Name;  // Ensure this is set correctly
            model.DateAdded = DateTime.Now;
            _context.Routes.Add(model);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Route added: {Name} by {User}", model.Name, model.RepresentativeUsername);
            return RedirectToAction("Index");
        }

        _logger.LogWarning("Model state invalid for route: {Name}", model.Name);
        foreach (var state in ModelState)
        {
            foreach (var error in state.Value.Errors)
            {
                _logger.LogWarning("Key: {Key}, Error: {ErrorMessage}", state.Key, error.ErrorMessage);
            }
        }
        return View(model);
    }
}
