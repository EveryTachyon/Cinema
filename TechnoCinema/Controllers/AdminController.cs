using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TechnoCinema.Data;
using TechnoCinema.Models;

namespace TechnoCinema.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db) => _db = db;

        // GET: /Admin/Movies  <-- this is your admin panel page
        public async Task<IActionResult> Movies()
        {
            var movies = await _db.Movies.ToListAsync();
            return View(movies);
        }

        // POST: /Admin/Movies  <-- handles the form submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Movies(Movie movie, string genres, string subtitles)
        {
            try
            {
                movie.Genres = genres?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(g => g.Trim()).ToList() ?? new List<string>();

                movie.Subtitles = subtitles?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                            .Select(s => s.Trim()).ToList() ?? new List<string>();

                _db.Movies.Add(movie);
                await _db.SaveChangesAsync();

                TempData["Success"] = "Movie added!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Movies");

        }
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _db.Movies.FindAsync(id);
            if (movie != null)
            {
                _db.Movies.Remove(movie);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Movie deleted!";
            }
            return RedirectToAction("Movies");
        }
    }
}