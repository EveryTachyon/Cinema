using Microsoft.AspNetCore.Mvc;
using TechnoCinema.Data;
using TechnoCinema.Models;
using System.Linq;
using System.Collections.Generic;

namespace TechnoCinema.Controllers
{
    public class MovieListController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieListController(ApplicationDbContext context)
        {
            _context = context;
        }
        private readonly List<string> AllGenres = new List<string>
{
    "Action", "Adventure", "Animation", "Biography", "Comedy", "Crime",
    "Documentary", "Drama", "Family", "Fantasy", "Film-Noir", "History",
    "Horror", "Music", "Musical", "Mystery", "Romance", "Sci-Fi",
    "Sport", "Thriller", "War", "Western", "Psychological", "Superhero",
    "Disaster", "Political", "Coming-of-Age", "Fantasy-Adventure",
    "Romantic-Comedy", "Slasher", "Time-Travel", "Cyberpunk", "Steampunk",
    "Epic", "Sword-and-Sorcery", "Martial-Arts", "Zombie", "Vampire",
    "Alien", "Post-Apocalyptic", "Survival", "Heist", "Spy", "Legal",
    "Medical", "Animated-Musical", "Dark-Comedy", "Buddy", "Mockumentary",
    "Adventure-Comedy", "Romantic-Drama", "Historical-Drama", "Teen",
    "Psychological-Thriller", "Horror-Comedy", "Family-Adventure",
    "Fantasy-Drama", "Action-Comedy", "Sci-Fi-Thriller"
};

        public IActionResult Index(
            string? searchTitle,
            List<string>? selectedGenres,
            string? sortBy)

        {
            var movies = _context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTitle))
            {
                movies = movies.Where(m => m.Title.Contains(searchTitle));
            }

            if (selectedGenres != null && selectedGenres.Any())
            {
                movies = movies.Where(m => m.Genres != null && m.Genres.Any(g => selectedGenres.Contains(g)));
            }

            movies = sortBy switch
            {
                "RatingDesc" => movies.OrderByDescending(m => m.Rating),
                "RatingAsc" => movies.OrderBy(m => m.Rating),
                _ => movies
            };

            ViewData["SearchTitle"] = searchTitle ?? "";
            ViewData["SortBy"] = sortBy ?? "";
            ViewData["SelectedGenres"] = selectedGenres ?? new List<string>();
            ViewData["AllGenres"] = AllGenres;

            return View(movies.ToList());
        }
    }
}

