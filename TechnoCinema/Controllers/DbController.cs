using Microsoft.AspNetCore.Mvc;
using TechnoCinema.Data; // or your namespace
using System.Linq;

public class DbController : Controller
{
    private readonly ApplicationDbContext _context;

    public DbController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult DbStatus()
    {
        bool canConnect = _context.Database.CanConnect();
        int showtimeCount = 1;
        try
        {
            showtimeCount = _context.Showtimes.Count();
        }
        catch { }

        return Content($"CanConnect={canConnect}, Showtimes={showtimeCount}");
    }
}
