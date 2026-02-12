using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TechnoCinema.Data;
using TechnoCinema.Models;

public class BuyController : Controller
{

    private readonly ApplicationDbContext _context;

    public BuyController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Buy/Index/{showtimeId}
    public IActionResult Index(int id)
    {
        var showtime = _context.Showtimes.FirstOrDefault(s => s.Id == id);
        if (showtime == null) return NotFound();

        var room = _context.Rooms
            .Include(r => r.Seats)
            .FirstOrDefault(r => r.Id == showtime.Saal);
        if (room == null) return NotFound();

        // Get booked seats for this showtime from Tickets
        var bookedSeatCodes = _context.Tickets
            .Where(t => t.Movie.Title == showtime.Film && t.Room.Id == showtime.Saal)
            .SelectMany(t => t.Seats.Select(s => $"{(char)('A' + s.Row)}{s.Column}"))
            .ToList();

        ViewBag.ShowtimeId = id;
        ViewBag.Showtime = showtime;
        ViewBag.BookedSeats = bookedSeatCodes;  // List of "A1", "B2", etc.

        return View();
    }

    // POST: /Buy/Confirm
    [HttpPost]
    public IActionResult Confirm(int showtimeId, string[] selectedSeats)
    {
        var showtime = _context.Showtimes.Find(showtimeId);
        if (showtime == null) return NotFound();

        var room = _context.Rooms
            .Include(r => r.Seats)
            .FirstOrDefault(r => r.Id == showtime.Saal);
        if (room == null) return NotFound();

        // Clear old tickets for this showtime (movie + room)
        var oldTickets = _context.Tickets
            .Where(t => t.Movie.Title == showtime.Film && t.Room.Id == showtime.Saal);
        _context.Tickets.RemoveRange(oldTickets);

        // Find actual Seat entities from selected seat codes (A1, B2, etc.)
        var seatsToBook = room.Seats
            .Where(s => selectedSeats.Contains($"{(char)('A' + s.Row)}{s.Column}"))
            .ToList();

        // Create new ticket
        var ticket = new Ticket
        {
            Movie = _context.Movies.FirstOrDefault(m => m.Title == showtime.Film),
            Room = room,
            Seats = seatsToBook,
            CreatedAt = DateTime.UtcNow
            // User = ... if you have authentication
        };

        _context.Tickets.Add(ticket);
        _context.SaveChanges();

        TempData["Message"] = "Seats booked successfully!";
        return RedirectToAction(nameof(Index), new { id = showtimeId });
    }

    // POST: /Buy/AdminReset
    [HttpPost]
    public IActionResult AdminReset(int showtimeId)
    {
        var showtime = _context.Showtimes.Find(showtimeId);
        if (showtime == null) return NotFound();

        // Remove all tickets for this showtime (movie + room)
        var tickets = _context.Tickets
            .Where(t => t.Movie.Title == showtime.Film && t.Room.Id == showtime.Saal);
        _context.Tickets.RemoveRange(tickets);
        _context.SaveChanges();

        return Json(new { success = true });
    }
}
