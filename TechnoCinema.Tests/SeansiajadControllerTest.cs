using Xunit;
using Microsoft.EntityFrameworkCore;
using TechnoCinema.Data;
using TechnoCinema.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

public class SeansiajadControllerTests
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }
    // ─── CREATE ────────────────────────────────────────

    [Fact]
    public void Create_ValidModel_Redirects()
    {
        var context = CreateContext();
        var controller = new SeansiajadController(context);

        var seans = new Showtime
        {
            Film = "Test",
            KinoNimi = "Apollo",
            ReleaseDate = DateTime.Now,
            VabuKohti = 10
        };

        var result = controller.Create(seans) as RedirectToActionResult;

        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }

    [Fact]
    public void Create_InvalidModel_ReturnsView()
    {
        var context = CreateContext();
        var controller = new SeansiajadController(context);

        controller.ModelState.AddModelError("Film", "Required");

        var result = controller.Create(new Showtime()) as ViewResult;

        Assert.NotNull(result);
    }

    // ─── EDIT ─────────────────────────────────────────

    [Fact]
    public void Edit_Get_ReturnsView_WhenExists()
    {
        var context = CreateContext();
        context.Showtimes.Add(new Showtime { Id = 1, Film = "Test", KinoNimi = "Apollo", ReleaseDate = DateTime.Now });
        context.SaveChanges();

        var controller = new SeansiajadController(context);

        var result = controller.Edit(1);

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Edit_Get_ReturnsNotFound_WhenMissing()
    {
        var controller = new SeansiajadController(CreateContext());

        var result = controller.Edit(999);

        Assert.IsType<NotFoundResult>(result);
    }

    // ─── OSTA PILETID ────────────────────────────────

    [Fact]
    public void OstaPiletid_ReturnsView_WhenExists()
    {
        var context = CreateContext();
        context.Showtimes.Add(new Showtime { Id = 1, Film = "Test", KinoNimi = "Apollo", ReleaseDate = DateTime.Now });
        context.SaveChanges();

        var controller = new SeansiajadController(context);

        var result = controller.OstaPiletid(1);

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void OstaPiletid_ReturnsNotFound_WhenMissing()
    {
        var controller = new SeansiajadController(CreateContext());

        var result = controller.OstaPiletid(999);

        Assert.IsType<NotFoundResult>(result);
    }
}