using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TechnoCinema.Data;
using TechnoCinema.Models;

public class DetailsController : Controller
{
    private readonly ApplicationDbContext _context;

    public DetailsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Details or /Details/{name}
    [Route("Details")]
    [Route("Details/{name}")]
    public IActionResult Index(string name)
    {
        // If name is null or empty, show all showtimes
        var showtimes = string.IsNullOrEmpty(name)
            ? _context.Showtimes.OrderBy(s => s.ReleaseDate).ToList()
            : _context.Showtimes
                .Where(s => s.Film.ToLower() == name.ToLower())
                .OrderBy(s => s.ReleaseDate)
                .ToList();

        if (!showtimes.Any())
            return NotFound();

        return View(showtimes);
    }
}
