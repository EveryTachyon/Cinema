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
        // GET ALL KINOS FROM DATABASE (no hardcoding)
        var kinod = _context.Showtimes
            .Select(s => s.KinoNimi)
            .Distinct()
            .ToList();

        ViewBag.Kinod = kinod;
        ViewBag.SelectedKinod = kino;
        ViewBag.SearchString = searchString;

        // BASE QUERY (ALL DATA FIRST)
        var query = _context.Showtimes.AsQueryable();

        // Filter by kino
        if (kino != null && kino.Count > 0 && !kino.Contains(""))
        {
            query = query.Where(s => kino.Contains(s.KinoNimi));
        }

        // Filter by film search
        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(s => s.Film.StartsWith(searchString));
        }

        // TOTAL COUNT (BEFORE PAGING)
        int totalItems = query.Count();

        // Pagination
        int pageSize = 5;
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



