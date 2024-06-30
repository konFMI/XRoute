using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using XRoute.Data;
using XRoute.Models;

[Authorize]
public class NewsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<NewsController> _logger;

    public NewsController(ApplicationDbContext context, ILogger<NewsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Authorize(Roles = "Representative, Normal")]
    [HttpGet]
    public IActionResult Index()
    {
        var news = _context.News.OrderByDescending(n => n.DatePosted).ToList();
        return View(news);
    }

    [Authorize(Roles = "Representative")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Representative")]
    [HttpPost]
    public async Task<IActionResult> Create(News model)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            _logger.LogInformation("User is authenticated: {User}", User.Identity.Name);
            model.RepresentativeUsername = User.Identity.Name ?? string.Empty;  // Ensure this is set correctly
        }
        else
        {
            _logger.LogWarning("User is not authenticated.");
        }

        // Recheck ModelState validity after setting RepresentativeUsername
        ModelState.Clear();
        TryValidateModel(model);

        if (ModelState.IsValid)
        {
            model.DatePosted = DateTime.Now;
            _context.News.Add(model);
            await _context.SaveChangesAsync();
            _logger.LogInformation("News item added: {Title} by {User}", model.Title, model.RepresentativeUsername);
            return RedirectToAction("Index");
        }

        _logger.LogWarning("Model state invalid for news item: {Title}", model.Title);
        foreach (var state in ModelState)
        {
            _logger.LogWarning("Key: {Key}, AttemptedValue: {AttemptedValue}, Errors: {Errors}",
                state.Key, state.Value.AttemptedValue, string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage)));
        }
        return View(model);
    }

    [Authorize(Roles = "Representative")]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var newsItem = await _context.News.FindAsync(id);
        if (newsItem != null)
        {
            _context.News.Remove(newsItem);
            await _context.SaveChangesAsync();
            _logger.LogInformation("News item deleted: {Title} by {User}", newsItem.Title, User.Identity?.Name);
        }
        else
        {
            _logger.LogWarning("News item not found: {Id}", id);
        }
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Representative, Normal")]
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var newsItem = await _context.News.FindAsync(id);
        if (newsItem == null)
        {
            return NotFound();
        }
        return View(newsItem);
    }
}
