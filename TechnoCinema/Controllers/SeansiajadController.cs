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

    // LIST ALL SHOWTIMES 
    //Andmete jagamine lehtedeks, andmete kuvamine lehtedena, pager view component 
    public IActionResult Index(List<string> kino, string searchString = "", int page = 1)
    {
        // ALL CINEMAS (cleaned + no nulls)
        var kinod = _context.Showtimes
            .Where(s => s.KinoNimi != null && s.KinoNimi.Trim() != "")
            .Select(s => s.KinoNimi.Trim())
            .Distinct()
            .OrderBy(k => k)
            .ToList();

        ViewBag.Kinod = kinod;
        ViewBag.SelectedKinod = kino ?? new List<string>();
        ViewBag.SearchString = searchString;

        // BASE QUERY
        var query = _context.Showtimes.AsQueryable();

        // FIX: kino filter (safe null + trim)
        if (kino != null && kino.Any(k => !string.IsNullOrWhiteSpace(k)))
        {
            var cleanKino = kino
                .Where(k => !string.IsNullOrWhiteSpace(k))
                .Select(k => k.Trim())
                .ToList();

            query = query.Where(s => s.KinoNimi != null && cleanKino.Contains(s.KinoNimi.Trim()));
        }

        // SEARCH FILTER
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            query = query.Where(s => s.Film.StartsWith(searchString));
        }

        // COUNT
        int totalItems = query.Count();

        // PAGING
        int pageSize = 5;

        var seansid = query
            .OrderBy(s => s.ReleaseDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        // COUNT PER KINO (cleaned same way)
        var kinoCounts = _context.Showtimes
            .Where(s => s.KinoNimi != null && s.KinoNimi.Trim() != "")
            .GroupBy(s => s.KinoNimi.Trim())
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



