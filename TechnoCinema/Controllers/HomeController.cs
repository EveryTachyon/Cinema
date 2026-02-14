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
        var banners = await _context.Banners
            .OrderBy(b => b.Position)
            .ToListAsync();

        return View(banners);
    }




    //  gives data do Edit Banner
    public async Task<IActionResult> Edit(int id)
    {
        var banner = await _context.Banners.FindAsync(id);

        if (banner == null)
            return NotFound();

        return View(banner);
    }

    //  Edit the Banner data 
    [HttpPost]
    public async Task<IActionResult> Edit(Banner banner)
    {
        if (ModelState.IsValid)
        {
            _context.Update(banner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Banner));
        }

        return View(banner);
    }


// Home page: show banners
public IActionResult Index()
    {
        var banners = _context.Banners.OrderBy(b => b.Position).ToList();
        return View(banners);
    }
}
