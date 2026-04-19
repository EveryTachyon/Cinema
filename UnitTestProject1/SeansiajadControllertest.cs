using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TechnoCinema.Data;
using TechnoCinema.Models;
using Xunit;

public class SeansiajadControllerTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

     
        context.Showtimes.AddRange(new List<Showtime>
        {
            new Showtime { Id = 1, Film = "Avatar", KinoNimi = "Tallinn", ReleaseDate = DateTime.Now },
            new Showtime { Id = 2, Film = "Batman", KinoNimi = "Tartu", ReleaseDate = DateTime.Now },
            new Showtime { Id = 3, Film = "Avatar 2", KinoNimi = "Tallinn", ReleaseDate = DateTime.Now }
        });

        context.SaveChanges();
        return context;
    }

    [Fact]
    public void Create_Get_Returns_View()
    {
        var controller = new SeansiajadController(GetDbContext());

        var result = controller.Create() as ViewResult;

        Assert.NotNull(result);
    }

    [Fact]
    public void Create_Post_Adds_Data_And_Redirects()
    {
        var context = GetDbContext();
        var controller = new SeansiajadController(context);

        var newSeans = new Showtime
        {
            Film = "NewMovie",
            KinoNimi = "Narva",
            ReleaseDate = DateTime.Now
        };

        var result = controller.Create(newSeans);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
        Assert.Contains(context.Showtimes, s => s.Film == "NewMovie");
    }

    [Fact]
    public void OstaPiletid_Returns_NotFound_If_Invalid()
    {
        var controller = new SeansiajadController(GetDbContext());

        var result = controller.OstaPiletid(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void OstaPiletid_Returns_View_If_Valid()
    {
        var controller = new SeansiajadController(GetDbContext());

        var result = controller.OstaPiletid(1) as ViewResult;

        Assert.NotNull(result);
    }

    [Fact]
    public void Index_Filters_By_Kino()
    {
        var controller = new SeansiajadController(GetDbContext());

        var result = controller.Index("Tallinn", "", 1) as ViewResult;
        var model = Assert.IsAssignableFrom<List<Showtime>>(result.Model);

        Assert.All(model, s => Assert.Equal("Tallinn", s.KinoNimi));
    }

    [Fact]
    public void Index_Search_Works()
    {
        var controller = new SeansiajadController(GetDbContext());

        var result = controller.Index("", "Avatar", 1) as ViewResult;
        var model = Assert.IsAssignableFrom<List<Showtime>>(result.Model);

        Assert.All(model, s => Assert.StartsWith("Avatar", s.Film));
    }

    [Fact]
    public void Edit_Get_Returns_NotFound_If_Invalid()
    {
        var controller = new SeansiajadController(GetDbContext());

        var result = controller.Edit(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Edit_Post_Updates_Data()
    {
        var context = GetDbContext();
        var controller = new SeansiajadController(context);

        var updated = context.Showtimes.First();
        updated.Film = "UpdatedMovie";

        var result = controller.Edit(updated.Id, updated);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);

        Assert.Equal("UpdatedMovie", context.Showtimes.First().Film);
    }
}