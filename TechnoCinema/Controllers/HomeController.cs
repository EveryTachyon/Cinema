using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TechnoCinema.Data;
using TechnoCinema.Models;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Home page: show banners
    public IActionResult Index()
    {
        var banners = _context.Banners.OrderBy(b => b.Position).ToList();
        return View(banners);
    }
}
