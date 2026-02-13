using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public async Task<IActionResult> Banner()
    {
        var banners = await _context.Banners.ToListAsync();
        return View(banners); // do not put a path, just View()
    }



    public IActionResult Edit()
    {
        return View();
    }
    // Home page: show banners
    public IActionResult Index()
    {
        var banners = _context.Banners.OrderBy(b => b.Position).ToList();
        return View(banners);
    }
}
