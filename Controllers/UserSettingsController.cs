using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XRoute.Data;
using XRoute.Models;
using XRoute.Utilities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class UserSettingsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserSettingsController> _logger;

    public UserSettingsController(ApplicationDbContext context, ILogger<UserSettingsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        string? userIdString = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString))
        {
            _logger.LogWarning("User ID is missing in session.");
            return RedirectToAction("Login", "Account");
        }

        if (!int.TryParse(userIdString, out int userId))
        {
            _logger.LogWarning("Invalid User ID format in session: {UserIdString}", userIdString);
            return BadRequest("Invalid User ID format.");
        }

        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            _logger.LogWarning("User not found with ID: {UserId}", userId);
            return NotFound();
        }

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateEmail(int id, string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            ModelState.AddModelError("Email", "Email cannot be empty.");
            return View("Index", await GetUserById(id));
        }

        var user = await GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            user.Email = email;
            _context.Update(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Email updated for user: {Email}", user.Email);
            return RedirectToAction("Index");
        }

        return View("Index", user);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfilePicture(int id, string profilePictureUrl)
    {
        var user = await GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            user.ProfilePictureUrl = profilePictureUrl;
            _context.Update(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Profile picture updated for user: {Email}", user.Email);
            return RedirectToAction("Index");
        }

        return View("Index", user);
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePassword(int id, string currentPassword, string newPassword)
    {
        if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
        {
            ModelState.AddModelError("", "Passwords cannot be empty.");
            return View("Index", await GetUserById(id));
        }

        var user = await GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (!PasswordHelper.VerifyPassword(currentPassword, user.PasswordHash, user.PasswordSalt))
            {
                ModelState.AddModelError("", "Current password is incorrect.");
                return View("Index", user);
            }

            string salt = PasswordHelper.CreateSalt();
            string hash = PasswordHelper.HashPassword(newPassword, salt);
            user.PasswordSalt = salt;
            user.PasswordHash = hash;

            _context.Update(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Password updated for user: {Email}", user.Email);
            return RedirectToAction("Index");
        }

        return View("Index", user);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        var user = await GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        _logger.LogInformation("User account deleted for user: {Email}", user.Email);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Login", "Account");
    }

    private async Task<User?> GetUserById(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            _logger.LogWarning("User not found with ID: {UserId}", id);
        }
        return user;
    }
}
