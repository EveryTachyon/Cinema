using Microsoft.AspNetCore.Mvc;
using TechnoCinema.Data;
using TechnoCinema.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace TechnoCinema.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _hasher;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<User>();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user, string password)
        {
            if (!ModelState.IsValid)
                return View(user);

            if (string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Password is required";
                return View(user);
            }

            user.Email = user.Email.ToLower();

            if (_context.Users.Any(u => u.Email == user.Email))
            {
                ViewBag.Error = "Email already exists";
                return View(user);
            }

            user.PasswordHash = _hasher.HashPassword(user, password);

            _context.Users.Add(user);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("UserId", user.Id);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == email.ToLower());

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password";
                return View("Login");
            }

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
            {
                ViewBag.Error = "Invalid email or password";
                return View("Login");
            }

            HttpContext.Session.SetInt32("UserId", user.Id);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}