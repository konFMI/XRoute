using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using XRoute.Data;
using XRoute.Models;
using XRoute.ViewModels;
using XRoute.Utilities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace XRoute.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ApplicationDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (user != null)
                {
                    if (PasswordHelper.VerifyPassword(model.Password, user.PasswordHash, user.PasswordSalt))
                    {
                        // Set the session
                        HttpContext.Session.SetString("UserId", user.Id.ToString());

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Role, user.Role)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties { IsPersistent = true };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid password.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterNormalUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNormalUser(RegisterNormalUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "User with this email already exists.");
                    return View(model);
                }

                string salt = PasswordHelper.CreateSalt();
                string hash = PasswordHelper.HashPassword(model.Password, salt);

                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordSalt = salt,
                    PasswordHash = hash,
                    Role = "Normal"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Normal user registered: {Email}", model.Email);
                return RedirectToAction("Login");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterRepresentative()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterRepresentative(RegisterRepresentativeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "User with this email already exists.");
                    return View(model);
                }

                string salt = PasswordHelper.CreateSalt();
                string hash = PasswordHelper.HashPassword(model.Password, salt);

                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordSalt = salt,
                    PasswordHash = hash,
                    Role = "Representative",
                    CompanyName = model.CompanyName,
                    CompanyAddress = model.CompanyAddress
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Representative user registered: {Email}", model.Email);
                return RedirectToAction("Login");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear(); // Clear the session
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
