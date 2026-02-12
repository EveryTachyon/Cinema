using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnoCinema.Data;
using TechnoCinema.Models;

public class SeansiajadController : Controller
{
    private readonly ApplicationDbContext _context;

    public SeansiajadController(ApplicationDbContext context)
    {
        _context = context;
    }
    

    // LIST ALL SHOWTIMES
    // GET: Create form
    public IActionResult Create()
    {
        return View();
    }

    // POST: Create Showtime
    [HttpPost]
    public IActionResult Create(Showtime uusSeans)
    {
        if (!ModelState.IsValid)
            return View(uusSeans);

        // EF will auto-generate ID because MySQL table should have AUTO_INCREMENT
        _context.Showtimes.Add(uusSeans);
        _context.SaveChanges();       

        return RedirectToAction("Index");
    }

    // Osta Piletid
    public IActionResult OstaPiletid(int id)
    {
        var seans = _context.Showtimes.FirstOrDefault(s => s.Id == id);

        if (seans == null)
            return NotFound();

        return View(seans);
    }

    // LIST ALL SHOWTIMES the white death 
    //Andmete jagamine lehtedeks, andmete kuvamine lehtedena, pager view component its 0245 and idk how it is 
    public IActionResult Index(string kino = "", string searchString = "", int page = 1)
    {
        // HARD-CODED kino locations
        var kinod = new List<string> { "Tallinn", "Tartu", "Pärnu", "Saaremaa", "Narva", "Jõhvi" };
        ViewBag.Kinod = kinod;
        ViewBag.SelectedKino = kino;
        ViewBag.SearchString = searchString;

        // Base query
        var query = _context.Showtimes.AsQueryable();

        // Filter by kino
        if (!string.IsNullOrEmpty(kino))
        {
            query = query.Where(s => s.KinoNimi == kino);
        }

        // Filter by film search
        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(s => s.Film.StartsWith(searchString));
        }

        // Pagination
        int pageSize = 5;
        int totalItems = query.Count();
        var seansid = query
            .OrderBy(s => s.ReleaseDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        // Count of seansid per kino
        var kinoCounts = _context.Showtimes
            .GroupBy(s => s.KinoNimi)
            .ToDictionary(g => g.Key, g => g.Count());
        ViewBag.KinoCounts = kinoCounts;

        return View(seansid);
    }


    //Edit seanss
    public IActionResult Edit(int id)
    {
        var seans = _context.Showtimes.Find(id);
        if (seans == null) return NotFound();
        return View(seans);
    }
    public IActionResult Edit2(int id)
    {
        var seans = _context.Showtimes.Find(id);
        if (seans == null) return NotFound();
        return View(seans);
    }

    public IActionResult EditedList()
    {
        var seansid = _context.Showtimes
            .OrderByDescending(s => s.ModifiedAt)
            .ToList();

        return View("EditedList", seansid);
    }



    [HttpPost]
    //[ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Showtime seans)
    {
        if (id != seans.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(seans);

        seans.ModifiedAt = DateTime.Now;

        _context.Showtimes.Update(seans);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }







}



