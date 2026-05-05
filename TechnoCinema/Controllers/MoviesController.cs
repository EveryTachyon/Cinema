using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnoCinema.Data;
using TechnoCinema.Models;

namespace TechnoCinema.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Movie List
        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies.ToListAsync();
            return View(movies);
        }

        // Movie Details
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound();

            var showtimes = await _context.Showtimes
                .Where(s => s.Film == movie.Title)
                .OrderBy(s => s.ReleaseDate)
                .ToListAsync();

            ViewBag.Showtimes = showtimes;

            return View(movie);
        }
        // GET: /Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Movie added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }
    }
}